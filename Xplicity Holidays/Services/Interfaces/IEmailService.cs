using System.Collections.Generic;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IEmailService
    {
        void ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        void ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus);
        void SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays);
        void InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays);
        void SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees);
    }
}
