using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        Task ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus, string overtimeSentence);
        Task SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays);
        Task InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays);
        Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees);
        Task<bool> SendOrderNotification(int fileId, Employee employee, string receiver);
        Task<bool> SendRequestNotification(int fileId, string receiver);
    }
}
