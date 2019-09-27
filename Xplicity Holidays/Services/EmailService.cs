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

        public void SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays)
        {
            var holidayInfo = string.Empty;
            var groupedHolidays = holidays.GroupBy(h => h.Item2);

            foreach (var client in groupedHolidays)
            {
                holidayInfo += $"{client.Key.CompanyName}'s team's this months holidays\r\n\r\n";
                foreach (var holiday in client)
                {
                    holidayInfo += $"   {holiday.Item1.Employee.Name} {holiday.Item1.Employee.Surname} was on holiday, from " +
                                   $"{holiday.Item1.FromInclusive.ToShortDateString()} to {holiday.Item1.ToExclusive.ToShortDateString()}, " +
                                   $"holiday type - {holiday.Item1.Type} \r\n";
                }
                holidayInfo += "\r\n";
            }

           _emailer.SendMail(admin.Email, "This months holiday summary", holidayInfo);
        }

        public void InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays)
        {
            foreach (var employee in employees)
            {
                foreach (var h in upcomingHolidays)
                {
                    if (h.Employee.Name != employee.Name && h.Employee.Surname != employee.Surname)
                    {
                        _emailer.SendMail(employee.Email, "Co-worker leaving on holiday",
                                    $"{h.Employee.Name} {h.Employee.Surname} is going on holiday next work day, from " +
                                         $"{h.FromInclusive.ToShortDateString()} to {h.ToExclusive.ToShortDateString()}");
                    }
                }
            }
        }

        public void SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees)
        {
            foreach (var employee in employees)
            {
                foreach (var employeeWithBirthday in employeesWithBirthdays)
                {
                    if (employee.Name != employeeWithBirthday.Name && employee.Surname != employeeWithBirthday.Surname)
                    {
                        _emailer.SendMail(employee.Email, "Birthday reminder", 
                                    $"Today is {employeeWithBirthday.Name} " + 
                                         $"{employeeWithBirthday.Surname} birthday today!\r\nMake sure to congratulate them.");
                    }
                }
            }
        }
    }
}
