﻿using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Interfaces
{
    public interface IEmailService
    {
        Task ConfirmHolidayWithClient(Client client, Employee employee, Holiday holiday);
        Task ConfirmHolidayWithAdmin(ICollection<Employee> admins, Employee employee, Holiday holiday, string clientStatus, string overtimeSentence);
        Task SendThisMonthsHolidayInfo(ICollection<Employee> admins, List<(Holiday, Client)> holidays);
        Task NotifyAllAboutUpcomingAbsences(ICollection<Employee> allEmployees, ICollection<Holiday> upcomingHolidays);
        Task SendBirthDayReminder(ICollection<Employee> employeesWithBirthdays, ICollection<Employee> employees);
        Task SendOrderNotification(int fileId, Employee employee, string receiver);
        Task<bool> SendRequestNotification(int fileId, string receiver, string confirmerFullName);
        Task NotifyAboutRejectedRequest(GetHolidayDto holiday, string receiver);
    }
}
