﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IEmailService
    {
        Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        Task ConfirmHolidayWithAdmin(Employee admin, Employee employee, Holiday holiday, string clientStatus);
        Task SendThisMonthsHolidayInfo(Employee admin, List<(Holiday, Client)> holidays);
        Task InformEmployeesAboutHoliday(ICollection<Employee> employees, ICollection<Holiday> upcomingHolidays);
        Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees);
    }
}
