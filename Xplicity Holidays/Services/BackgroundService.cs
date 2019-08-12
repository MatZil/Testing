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
        private readonly ITimeService _timeservice;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundService(ITimeService timeService, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _timeservice = timeService;
        }

        public async Task RunBackgroundServices()
        {
            while (true)
            {
                await DoBackGroundChecks();

                await Task.Delay(TimeSpan.FromDays(1));
            }
        }

        public async Task DoBackGroundChecks()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();
                var holidayRepository = scope.ServiceProvider.GetService<IRepository<Holiday>>();
                var emailService = scope.ServiceProvider.GetService<IEmailService>();

                var holidays = await holidayRepository.GetAll();
                var employees = await employeeRepository.GetAll();
                var admin = await employeeRepository.FindAnyAdmin();

                await Task.Run(() => CheckForLastMonthDay(admin, holidays, emailService));

                await Task.Run(() => CheckUpcomingHolidays(employees, holidays, emailService));
            }
        }

        public void CheckForLastMonthDay(Employee admin, ICollection<Holiday> holidays, IEmailService emailService)
        {
            var currentTime = _timeservice.GetCurrentTime();
            var thisMonthsHolidays = holidays.Where(h => h.IsConfirmed == true && (h.FromInclusive.Year == currentTime.Year
                                                  || h.ToExclusive.Year == currentTime.AddDays(1).Year)
                                                  && (h.FromInclusive.Month == currentTime.Month
                                                  || h.ToExclusive.Month == currentTime.AddDays(1).Month)).ToList();

            DateTime nextDay = currentTime.AddDays(1);

            if (currentTime.Month != nextDay.Month)
            {
                //emailService.SendThisMonthsHolidayInfo(admin, thisMonthsHolidays);
            }
        }

        public void CheckUpcomingHolidays(ICollection<Employee> employees, ICollection<Holiday> holidays, IEmailService emailService)
        {
            DateTime currentTime = _timeservice.GetCurrentTime();

            var upcomingHolidays = holidays.Where(holiday => holiday.IsConfirmed == true
                                                  && holiday.FromInclusive.ToShortDateString() == currentTime.AddDays(1).ToShortDateString())
                                                  .ToList();

            if (upcomingHolidays.Count != 0)
            {
                //emailService.InformEmployeesAboutHoliday(employees, upcomingHolidays);
            }
        }


    }
}
