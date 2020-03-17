using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Emailer;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Services.EntityBehavior;
using XplicityApp.Services.Interfaces;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailer _emailer;
        private readonly IEmailTemplatesRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IOvertimeUtility _overtimeUtility;

        public EmailService(IEmailer emailer, IEmailTemplatesRepository repository, IConfiguration configuration, IFileService fileService, IOvertimeUtility overtimeUtility)
        {
            _emailer = emailer;
            _repository = repository;
            _configuration = configuration;
            _fileService = fileService;
            _overtimeUtility = overtimeUtility;
        }

        public async Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.CLIENT_CONFIRMATION);
            var messageString = template.Template
                                        .Replace("{client.name}", client.OwnerName)
                                        .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToInclusive.ToShortDateString())
                                        .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayClient?holidayId={holiday.Id}&confirmerId={client.Id}")
                                        .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={holiday.Id}&confirmerId={client.Id}");

            _emailer.SendMail(client.OwnerEmail, template.Subject, messageString);
        }

        public async Task ConfirmHolidayWithAdmin(ICollection<Employee> admins, Employee employee, Holiday holiday, string clientStatus, string overtimeSentence)
        {

            foreach (var admin in admins)
            {
                var template = await _repository.GetByPurpose(EmailPurposes.ADMIN_CONFIRMATION);
                var messageString = template.Template
                                            .Replace("{admin.name}", admin.Name)
                                            .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                            .Replace("{holiday.paid}", holiday.Paid ? "Paid" : "Unpaid")
                                            .Replace("{holiday.type}", holiday.Type.ToString())
                                            .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                            .Replace("{holiday.to}", holiday.ToInclusive.ToShortDateString())
                                            .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayConfirm?holidayId={holiday.Id}&confirmerId={admin.Id}")
                                            .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={holiday.Id}&confirmerId={admin.Id}")
                                            .Replace("{client.status}", clientStatus)
                                            .Replace("{holiday.overtimeHours}", overtimeSentence);

                _emailer.SendMail(admin.Email, template.Subject, messageString); 
            }
        }

        public async Task SendThisMonthsHolidayInfo(ICollection<Employee> admins, List<(Holiday, Client)> holidays)
        {
            var holidayInfo = string.Empty;
            var groupedHolidays = holidays.GroupBy(h => h.Item2);
            var template = await _repository.GetByPurpose(EmailPurposes.MONTHLY_HOLIDAYS_REPORT);
            int titleEnd = template.Template.IndexOf('\n', 0);
            foreach (var client in groupedHolidays)
            {
                holidayInfo += template.Template.Substring(0, titleEnd)
                                                .Replace("{client.name}", (client.Key is null) ? TeamlessEmployeeTitle.TEAM_NAME : client.Key.CompanyName) + '\n';
                foreach (var holiday in client)
                {
                    var overtimeSentence = _overtimeUtility.GetOvertimeSentence(OvertimeEmail.MONTHLY_REPORT, holiday.Item1.OvertimeDays);

                    var messageString = template.Template.Substring(titleEnd)
                                                        .Replace("{employee.fullName}", $"{holiday.Item1.Employee.Name} {holiday.Item1.Employee.Surname}")
                                                        .Replace("{holiday.paid}", holiday.Item1.Paid ? "Paid" : "Unpaid")
                                                        .Replace("{holiday.type}", holiday.Item1.Type.ToString())
                                                        .Replace("{holiday.from}", holiday.Item1.FromInclusive.ToShortDateString())
                                                        .Replace("{holiday.to}", holiday.Item1.ToInclusive.ToShortDateString())
                                                        .Replace("{holiday.overtimeHours}", overtimeSentence);
                    holidayInfo += messageString;
                }
                holidayInfo += "\n\n";
            }

            foreach (var admin in admins)
            {
                _emailer.SendMail(admin.Email, template.Subject, holidayInfo);
            }
        }

        public async Task NotifyAllAboutUpcomingAbsences(ICollection<Employee> allEmployees, ICollection<Holiday> upcomingHolidays)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.HOLIDAY_REMINDER);
            foreach (var recipient in allEmployees)
            {
                var messageBuilder = new StringBuilder();
                foreach (var upcomingHoliday in upcomingHolidays)
                {
                    if (!upcomingHoliday.Employee.IsSamePerson(recipient))
                    {
                        messageBuilder.AppendLine(template.Template
                                                    .Replace("{employee.fullName}", $"{upcomingHoliday.Employee.Name} {upcomingHoliday.Employee.Surname}")
                                                    .Replace("{holiday.from}", upcomingHoliday.FromInclusive.ToShortDateString())
                                                    .Replace("{holiday.to}", upcomingHoliday.ToInclusive.ToShortDateString()));
                    }
                }

                var messageBody = messageBuilder.ToString();
                _emailer.SendMail(recipient.Email, template.Subject, messageBody);
            }
        }

        public async Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.BIRTHDAY_REMINDER);
            foreach (var employee in employees)
            {
                if (employee.NotificationSettings.ReceiveBirthdayNotifications == true)
                {
                    foreach (var employeeWithBirthday in employeesWithBirthdays)
                    {
                        if (!employeeWithBirthday.IsSamePerson(employee))
                        {
                            var messageString = template.Template
                                                        .Replace("{employee.fullName}", $"{employeeWithBirthday.Name} {employeeWithBirthday.Surname}");
                            _emailer.SendMail(employee.Email, template.Subject, messageString);
                        }
                    }
                }
            }
        }

        public async Task SendOrderNotification(int fileId, Employee employee, string receiver)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.ORDER_NOTIFICATION);

            if (template is null)
            {
                throw new InvalidOperationException($"{EmailPurposes.ORDER_NOTIFICATION} template was not found.");
            }

            var messageString = template.Template
                                        .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                        .Replace("{download.link}", _fileService.GetDownloadLink(fileId));

            _emailer.SendMail(receiver, template.Subject, messageString);
        }

        public async Task<bool> SendRequestNotification(int fileId, string receiver, string confirmerFullName)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.REQUEST_NOTIFICATION);

            if (template is null)
            {
                return false;
            }

            var messageString = template.Template
                                        .Replace("{confirmer.fullName}", confirmerFullName)
                                        .Replace("{download.link}", _fileService.GetDownloadLink(fileId));

            _emailer.SendMail(receiver, template.Subject, messageString);

            return true;
        }
    }
}
