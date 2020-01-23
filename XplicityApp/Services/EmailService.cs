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

namespace XplicityApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailer _emailer;
        private readonly IEmailTemplatesRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public EmailService(IEmailer emailer, IEmailTemplatesRepository repository, IConfiguration configuration, IFileService fileService)
        {
            _emailer = emailer;
            _repository = repository;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.CLIENT_CONFIRMATION);
            var messageString = template.Template
                                        .Replace("{client.name}", client.OwnerName)
                                        .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.AddDays(-1).ToShortDateString())
                                        .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayClient?holidayId={holiday.Id}")
                                        .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={holiday.Id}");

            _emailer.SendMail(client.OwnerEmail, template.Subject, messageString);
        }

        public async Task ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus)
        {
            var overtimeString = "";
            if (holiday.OvertimeDays > 0)
            {
                overtimeString = " (using " + Math.Round(holiday.OvertimeHours, 2) + " overtime hours)";
            }

            var template = await _repository.GetByPurpose(EmailPurposes.ADMIN_CONFIRMATION);
            var messageString = template.Template
                                        .Replace("{admin.name}", admin.Name)
                                        .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                        .Replace("{holiday.paid}", holiday.Paid ? "Paid" : "Unpaid")
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.AddDays(-1).ToShortDateString())
                                        .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayConfirm?holidayId={holiday.Id}")
                                        .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={holiday.Id}")
                                        .Replace("{client.status}", clientStatus)
                                        .Replace("{holiday.overtimeHours}", overtimeString);

            _emailer.SendMail(admin.Email, template.Subject, messageString);
        }

        public async Task SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays)
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
                    var messageString = template.Template.Substring(titleEnd)
                                                        .Replace("{employee.fullName}", $"{holiday.Item1.Employee.Name} {holiday.Item1.Employee.Surname}")
                                                        .Replace("{holiday.paid}", holiday.Item1.Paid ? "Paid" : "Unpaid")
                                                        .Replace("{holiday.type}", holiday.Item1.Type.ToString())
                                                        .Replace("{holiday.from}", holiday.Item1.FromInclusive.ToShortDateString())
                                                        .Replace("{holiday.to}", holiday.Item1.ToExclusive.AddDays(-1).ToShortDateString())
                                                        .Replace("{holiday.overtimeHours}", holiday.Item1.OvertimeHours.ToString());
                    holidayInfo += messageString;
                }
                holidayInfo += "\n\n";
            }
            _emailer.SendMail(admin.Email, template.Subject, holidayInfo);
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
                                                    .Replace("{holiday.to}", upcomingHoliday.ToExclusive.AddDays(-1).ToShortDateString()));
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

        public async Task<bool> SendOrderNotification(int fileId, Employee employee, string receiver)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.ORDER_NOTIFICATION);

            if (template is null)
            {
                return false;
            }

            var messageString = template.Template
                                        .Replace("{employee.fullName}", $"{employee.Name} {employee.Surname}")
                                        .Replace("{download.link}", _fileService.GetDownloadLink(fileId));

            _emailer.SendMail(receiver, template.Subject, messageString);

            return true;
        }

        public async Task<bool> SendRequestNotification(int fileId, string receiver)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.REQUEST_NOTIFICATION);

            if (template is null)
            {
                return false;
            }

            var messageString = template.Template.Replace("{download.link}", _fileService.GetDownloadLink(fileId));

            _emailer.SendMail(receiver, template.Subject, messageString);

            return true;
        }
    }
}
