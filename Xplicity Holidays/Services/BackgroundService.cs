using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly ITimeService _timeService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BackgroundService(ITimeService timeService, IServiceScopeFactory serviceScopeFactory, IHostingEnvironment hostingEnvironment)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _timeService = timeService;
            _hostingEnvironment = hostingEnvironment;
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
                var admin = await employeeRepository.FindAnyAdmin();

                await CheckForLastMonthDay(admin, holidays, emailService, holidayInfoService);

                await CheckUpcomingHolidays(employees, holidays, emailService);

                await CheckBirthdays(employees, _timeService, emailService);

                await AddFreeWorkDays(employees, _timeService, employeeRepository);

                await ResetParentalLeaves(employees, _timeService, employeeRepository);
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

        private async Task CheckForLastMonthDay(Employee admin, ICollection<Holiday> holidays, IEmailService emailService,
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
                await emailService.SendThisMonthsHolidayInfo(admin, holidaysWithClients);
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
                await emailService.InformEmployeesAboutHoliday(employees, upcomingHolidays);
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

        private async Task AddFreeWorkDays(ICollection<Employee> employees, ITimeService _timeService, IEmployeeRepository _repository)
        {
            var currentTime = _timeService.GetCurrentTime();
            if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
            {
                var workDaysPerYear = _timeService.GetWorkDays(new DateTime(currentTime.Year, 1, 1),
                                                            new DateTime(currentTime.AddYears(1).Year, 1, 1));
                foreach (var employee in employees)
                {
                    employee.FreeWorkDays += Math.Round((double)employee.DaysOfVacation / workDaysPerYear, 2);
                    await _repository.Update(employee);
                }
            }
        }

        private async Task ResetParentalLeaves(ICollection<Employee> employees, ITimeService timeService, IEmployeeRepository repository)
        {
            var currentTime = timeService.GetCurrentTime();
            if (currentTime.Day == 1)
            {
                foreach (var employee in employees)
                {
                    employee.CurrentAvailableLeaves = employee.NextMonthAvailableLeaves;
                    employee.NextMonthAvailableLeaves = employee.ParentalLeaveLimit;
                    await repository.Update(employee);
                }
            }
        }

    }
}
