using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly ITimeService _timeService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundService(ITimeService timeService, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _timeService = timeService;
        }

        public async Task RunBackgroundServices()
        {
            while (true)
            {
              //  await DoBackGroundChecks();

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        private async Task DoBackGroundChecks()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();
                var holidayRepository = scope.ServiceProvider.GetService<IRepository<Holiday>>();
                var emailService = scope.ServiceProvider.GetService<IEmailService>();
                var holidayInfoService = scope.ServiceProvider.GetService<IHolidayInfoService>();

                var holidays = await holidayRepository.GetAll();
                var employees = await employeeRepository.GetAll();
                var admin = await employeeRepository.FindAnyAdmin();

                await Task.Run(() => CheckForLastMonthDay(admin, holidays, emailService, holidayInfoService));

                await Task.Run(() => CheckUpcomingHolidays(employees, holidays, emailService));

                await Task.Run(() => CheckBirthdays(employees, _timeService, emailService));
            }
        }

        private void CheckForLastMonthDay(Employee admin, ICollection<Holiday> holidays, IEmailService emailService,
                                          IHolidayInfoService holidayInfoService)
        {
            //var currentTime = _timeService.GetCurrentTime();
            var currentTime = new DateTime(2019, 8, 31);
            var thisMonthsHolidays = holidays.Where(h => h.Status == "Confirmed" && (h.FromInclusive.Year == currentTime.Year
                                                  || h.ToExclusive.AddDays(-1).Year == currentTime.Year)
                                                  && (h.FromInclusive.Month == currentTime.Month
                                                  || h.ToExclusive.AddDays(-1).Month == currentTime.Month)).ToList();

            var holidaysWithClients = holidayInfoService.GetClientsAndHolidays(thisMonthsHolidays).Result;

            var nextDay = currentTime.AddDays(1);

            if(currentTime.Month != nextDay.Month)
                emailService.SendThisMonthsHolidayInfo(admin, holidaysWithClients);
        }

        private void CheckUpcomingHolidays(ICollection<Employee> employees, ICollection<Holiday> holidays, IEmailService emailService)
        {
            var currentTime = _timeService.GetCurrentTime();

            var upcomingHolidays = holidays.Where(holiday => holiday.Status == "Confirmed" && 
                                                 holiday.FromInclusive.ToShortDateString() == currentTime.AddDays(1).ToShortDateString())
                                                  .ToList();

            if (upcomingHolidays.Count != 0)
                emailService.InformEmployeesAboutHoliday(employees, upcomingHolidays);
        }

        private void CheckBirthdays(ICollection<Employee> employees, ITimeService _timeService, IEmailService emailService)
        {
            var employeesWithBirthdays = new List<Employee>();
            var currentTime = _timeService.GetCurrentTime();

            foreach (var employee in employees)
            {
                if (employee.BirthdayDate.Month == currentTime.Month && employee.BirthdayDate.Day == currentTime.Day)
                    employeesWithBirthdays.Add(employee);
            }

            if (employeesWithBirthdays.Count != 0)
                emailService.SendBirthDayReminder(employeesWithBirthdays, employees);
        }


    }
}
