using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        Task ConfirmHolidayWithAdmin(ICollection<Employee> admin, Employee employee, Holiday holiday, string clientStatus);
        Task SendThisMonthsHolidayInfo(ICollection<Employee> admin, List<(Holiday, Client)> holidays);
        Task InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays);
        Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees);
        Task SendOrderNotification(int fileId, Employee employee, string receiver);
        Task<bool> SendRequestNotification(int fileId, string receiver);
    }
}
