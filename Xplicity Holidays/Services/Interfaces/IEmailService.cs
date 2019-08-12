using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IEmailService
    {
        void ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        void ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus);
        void SendThisMonthsHolidayInfo(Employee admin, List<Holiday> holidays);
        void InformEmployeesAboutHoliday(List<Employee> employees, List<Holiday> upcomingHolidays);
    }
}
