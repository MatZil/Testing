using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using XplicityApp.Services.Extensions.Interfaces;

namespace XplicityApp.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly ITimeService _timeService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;

        public BackgroundService(ITimeService timeService, IServiceScopeFactory serviceScopeFactory, IWebHostEnvironment hostingEnvironment,
                                 IEmployeeHolidaysBackgroundUpdater employeeHolidaysBackgroundUpdater)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _timeService = timeService;
            _hostingEnvironment = hostingEnvironment;
            _employeeHolidaysBackgroundUpdater = employeeHolidaysBackgroundUpdater;
        }

        public async Task RunBackgroundServices()
        {
            while (true)
            {
                await DoBackGroundChecks();

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        private async Task DoBackGroundChecks()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();
                var holidayRepository = scope.ServiceProvider.GetService<IHolidaysRepository>();
                var emailService = scope.ServiceProvider.GetService<IEmailService>();
                var holidayInfoService = scope.ServiceProvider.GetService<IHolidayInfoService>();

                var holidays = await holidayRepository.GetAll();
                var employees = await employeeRepository.GetAll();
                var admins = await employeeRepository.GetAllAdmins();

                await CheckForLastMonthDay(admins, holidays, emailService, holidayInfoService);

                await CheckUpcomingHolidays(employees, holidays, emailService);

                await CheckBirthdays(employees, _timeService, emailService);

                await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(employees, _timeService, employeeRepository);

                await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(employees, _timeService, employeeRepository);
            }
        }

        private DateTime GetCurrentDateTime()
        {
            DateTime currentTime;

            if (_hostingEnvironment.IsProduction())
            {
                currentTime = _timeService.GetCurrentTime();
            }
            else
            {
                currentTime = DateTime.MinValue; //makes sure that it's not month's last day.
            }

            return currentTime;
        }

        private async Task CheckForLastMonthDay(ICollection<Employee> admins, ICollection<Holiday> holidays, IEmailService emailService,
                                          IHolidayInfoService holidayInfoService)
        {
            var currentTime = GetCurrentDateTime();

            var thisMonthsHolidays = holidays.Where(h => 
                                                        h.Status == HolidayStatus.Confirmed && 
                                                        h.FromInclusive.Year == currentTime.Year && 
                                                        h.FromInclusive.Month == currentTime.Month
                                                    ).ToList();

            var holidaysWithClients = await holidayInfoService.GetClientsAndHolidays(thisMonthsHolidays);

            var nextDay = currentTime.AddDays(1);

            if (currentTime.Month != nextDay.Month)
            {
                await emailService.SendThisMonthsHolidayInfo(admins, holidaysWithClients);
            }
        }

        private async Task CheckUpcomingHolidays(ICollection<Employee> employees, ICollection<Holiday> holidays, IEmailService emailService)
        {
            var currentTime = GetCurrentDateTime();

            var upcomingHolidays = holidays.Where(holiday => holiday.Status == HolidayStatus.Confirmed && 
                                                 holiday.FromInclusive.ToShortDateString() == currentTime.AddDays(1).ToShortDateString())
                                                  .ToList();

            if (upcomingHolidays.Count != 0)
            {
                await emailService.NotifyAllAboutUpcomingAbsences(employees, upcomingHolidays);
            }
        }

        private async Task CheckBirthdays(ICollection<Employee> employees, ITimeService _timeService, IEmailService emailService)
        {
            var employeesWithBirthdays = new List<Employee>();
            var currentTime = GetCurrentDateTime();

            foreach (var employee in employees)
            {
                if (employee.BirthdayDate.Month == currentTime.Month && employee.BirthdayDate.Day == currentTime.Day)
                {
                    employeesWithBirthdays.Add(employee);
                }
            }

            if (employeesWithBirthdays.Count != 0)
            {
                await emailService.SendBirthDayReminder(employeesWithBirthdays, employees);
            }
        }

    }
}
