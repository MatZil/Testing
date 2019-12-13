using System.Collections.Generic;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Services.Interfaces;
using System.Linq;
using Xplicity_Holidays.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Static_Files;
using System;

namespace Xplicity_Holidays.Services
{
    public class EmailService: IEmailService
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
                                        .Replace("{employee.name}", employee.Name)
                                        .Replace("{employee.surname}", employee.Surname)
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.ToShortDateString())
                                        .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/holidayclient?holidayid={holiday.Id}")
                                        .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/holidaydecline?holidayid={holiday.Id}");

            _emailer.SendMail(client.OwnerEmail, template.Subject, messageString);
        }

        private string GetOvertimeSentence(Holiday holiday)
        {
            var overtimeSentence = "";
            if (holiday.OvertimeDays > 0)
            {
                overtimeSentence = " (using " + Math.Round(holiday.OvertimeHours, 2).ToString() + " overtime hours)";
            }

            return overtimeSentence;
        }

        public async Task ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.ADMIN_CONFIRMATION);
            var messageString = template.Template
                                        .Replace("{admin.name}", admin.Name)
                                        .Replace("{employee.name}", employee.Name)
                                        .Replace("{employee.surname}", employee.Surname)
                                        .Replace("{employee.paid}", holiday.Paid ? "Paid" : "Unpaid")
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.ToShortDateString())
                                        .Replace("{holiday.confirm}", $"{_configuration["AppSettings:RootUrl"]}/api/holidayconfirm?holidayid={holiday.Id}")
                                        .Replace("{holiday.decline}", $"{_configuration["AppSettings:RootUrl"]}/api/holidaydecline?holidayid={holiday.Id}")
                                        .Replace("{client.status}", clientStatus)
                                        .Replace("{holiday.overtimeHours}", GetOvertimeSentence(holiday));

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
                                                        .Replace("{employee.name}", holiday.Item1.Employee.Name)
                                                        .Replace("{employee.surname}", holiday.Item1.Employee.Surname)
                                                        .Replace("{employee.paid}", holiday.Item1.Paid ? "Paid" : "Unpaid")
                                                        .Replace("{holiday.type}", holiday.Item1.Type.ToString())
                                                        .Replace("{holiday.from}", holiday.Item1.FromInclusive.ToShortDateString())
                                                        .Replace("{holiday.to}", holiday.Item1.ToExclusive.ToShortDateString())
                                                        .Replace("{holiday.overtimeHours}", GetOvertimeSentence(holiday.Item1));
                    holidayInfo += messageString;
                }
                holidayInfo += "\n\n";
            }
            _emailer.SendMail(admin.Email, template.Subject, holidayInfo);
        }

        public async Task InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.HOLIDAY_REMINDER);
            foreach (var employee in employees)
            {
                foreach (var h in upcomingHolidays)
                {
                    if (h.Employee.Name != employee.Name && h.Employee.Surname != employee.Surname)
                    {
                        var messageString = template.Template
                                                    .Replace("{employee.name}", h.Employee.Name)
                                                    .Replace("{employee.surname}", h.Employee.Surname)
                                                    .Replace("{holiday.from}", h.FromInclusive.ToShortDateString())
                                                    .Replace("{holiday.to}", h.ToExclusive.ToShortDateString());

                        _emailer.SendMail(employee.Email, template.Subject, messageString);
                    }
                }
            }
        }

        public async Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees)
        {
            var template = await _repository.GetByPurpose(EmailPurposes.BIRTHDAY_REMINDER);
            foreach (var employee in employees)
            {
                foreach (var employeeWithBirthday in employeesWithBirthdays)
                {
                    if (employee.Name != employeeWithBirthday.Name && employee.Surname != employeeWithBirthday.Surname)
                    {
                        var messageString = template.Template
                                                    .Replace("{employee.name}", employeeWithBirthday.Name)
                                                    .Replace("{employee.surname}", employeeWithBirthday.Surname);
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
                                        .Replace("{employee.name}", employee.Name)
                                        .Replace("{employee.surname}", employee.Surname)
                                        .Replace("{download_link}", _fileService.GetDownloadLink(fileId));

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

            var messageString = template.Template.Replace("{download_link}", _fileService.GetDownloadLink(fileId));

            _emailer.SendMail(receiver, template.Subject, messageString);

            return true;
        }
    }
}
