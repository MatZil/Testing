using System.Collections.Generic;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Emailer;
using Xplicity_Holidays.Services.Interfaces;
using System.Linq;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Services
{
    public class EmailService: IEmailService
    {
        private readonly IEmailer _emailer;
        private readonly IEmailTemplatesRepository _repository;

        public EmailService(IEmailer emailer, IEmailTemplatesRepository repository)
        {
            _emailer = emailer;
            _repository = repository;
        }

        public async void ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday)
        {
            var template = await _repository.GetByPurpose("Client Confirmation");
            var messageString = template.Template
                                        .Replace("{client.email}", client.OwnerEmail)
                                        .Replace("{client.name}", client.OwnerName)
                                        .Replace("{employee.name}", employee.Name)
                                        .Replace("{employee.surname}", employee.Surname)
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.ToShortDateString())
                                        .Replace("{holiday.confirm}", $"https://localhost:44374/api/holidayclient?holidayid={holiday.Id}")
                                        .Replace("{holiday.decline}", $"https://localhost:44374/api/holidaydecline?holidayid={holiday.Id}");

            _emailer.SendMail(client.OwnerEmail, template.Subject, messageString);
        }

        public async void ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus)
        {
            var template = await _repository.GetByPurpose("Admin Confirmation");
            var messageString = template.Template
                                        .Replace("{admin.name}", admin.Name)
                                        .Replace("{employee.name}", employee.Name)
                                        .Replace("{employee.surname}", employee.Surname)
                                        .Replace("{holiday.type}", holiday.Type.ToString())
                                        .Replace("{holiday.from}", holiday.FromInclusive.ToShortDateString())
                                        .Replace("{holiday.to}", holiday.ToExclusive.ToShortDateString())
                                        .Replace("{holiday.confirm}", $"https://localhost:44374/api/holidayconfirm?holidayid={holiday.Id}")
                                        .Replace("{holiday.decline}", $"https://localhost:44374/api/holidaydecline?holidayid={holiday.Id}")
                                        .Replace("{client.status}", clientStatus);

            _emailer.SendMail(admin.Email, template.Subject, messageString);
        }

        public async void SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays)
        {
            var holidayInfo = string.Empty;
            var groupedHolidays = holidays.GroupBy(h => h.Item2);
            var template = await _repository.GetByPurpose("Monthly Holidays' Report");
            int titleEnd = template.Template.IndexOf('\n', 0);
            foreach (var client in groupedHolidays)
            {
                holidayInfo += template.Template.Substring(0, titleEnd).Replace("{client.name}", client.Key.CompanyName) + '\n';
                foreach (var holiday in client)
                {
                    var messageString = template.Template.Substring(titleEnd)
                                                        .Replace("{employee.name}", holiday.Item1.Employee.Name)
                                                        .Replace("{employee.surname}", holiday.Item1.Employee.Surname)
                                                        .Replace("{holiday.type}", holiday.Item1.Type.ToString())
                                                        .Replace("{holiday.from}", holiday.Item1.FromInclusive.ToShortDateString())
                                                        .Replace("{holiday.to}", holiday.Item1.ToExclusive.ToShortDateString());
                    holidayInfo += messageString;
                }
                holidayInfo += "\n\n";
            }
            _emailer.SendMail(admin.Email, template.Subject, holidayInfo);
        }

        public async void InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays)
        {
            var template = await _repository.GetByPurpose("Upcoming Holiday Reminder");
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

        public async void SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees)
        {
            var template = await _repository.GetByPurpose("Birthday Reminder");
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
    }
}
