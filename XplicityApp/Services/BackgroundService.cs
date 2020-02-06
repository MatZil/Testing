using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class BackgroundService : IBackgroundService
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmailService _emailService;
        private readonly IHolidayInfoService _holidayInfoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<BackgroundService> _logger;

        public BackgroundService(ITimeService timeService, IEmployeeHolidaysBackgroundUpdater employeeHolidaysBackgroundUpdater,
                            IEmployeeRepository employeeRepository, IHolidaysRepository holidaysRepository,
                            IEmailService emailService, IHolidayInfoService holidayInfoService,
                            IWebHostEnvironment webHostEnvironment, ILogger<BackgroundService> logger)
        {
            _timeService = timeService;
            _employeeHolidaysBackgroundUpdater = employeeHolidaysBackgroundUpdater;
            _employeeRepository = employeeRepository;
            _holidaysRepository = holidaysRepository;
            _emailService = emailService;
            _holidayInfoService = holidayInfoService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        public async Task DoBackgroundTasks()
        {
            _logger.LogInformation("Background tasks were initiated at " + _timeService.GetCurrentTime());

            await SendHolidayReports();

            await BroadcastCoworkersAbsences();

            await BroadcastCoworkersBirthdays();

            var allEmployees = await _employeeRepository.GetAll();

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(allEmployees);

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(allEmployees);

            _logger.LogInformation("Background tasks ended at " + _timeService.GetCurrentTime());
        }

        private DateTime GetCurrentDateTime()
        {
            DateTime currentTime;

            if (_webHostEnvironment.IsProduction())
            {
                currentTime = _timeService.GetCurrentTime();
            }
            else
            {
                currentTime = DateTime.MinValue; //makes sure background tasks are not triggered.
            }

            return currentTime;
        }

        private async Task SendHolidayReports()
        {
            _logger.LogInformation("SendHolidayReports() was initiated at " + _timeService.GetCurrentTime());

            var currentTime = GetCurrentDateTime();

            if (currentTime.Month != currentTime.AddDays(1).Month)
            {
                try
                {
                    var allHolidays = await _holidaysRepository.GetAll();
                    var currentMonthHolidays = allHolidays.Where(h =>
                                                                    h.Status == HolidayStatus.Confirmed &&
                                                                    h.FromInclusive.Year == currentTime.Year &&
                                                                    h.FromInclusive.Month == currentTime.Month
                                                                ).ToList();

                    var holidaysWithClients = await _holidayInfoService.GetClientsAndHolidays(currentMonthHolidays);
                    var admins = await _employeeRepository.GetAllAdmins();

                    await _emailService.SendThisMonthsHolidayInfo(admins, holidaysWithClients);
                }
                catch (Exception exception)
                {
                    _logger.LogInformation(exception.ToString() + " occurred in SendHolidayReports() at " + _timeService.GetCurrentTime());
                }
            }

            _logger.LogInformation("SendHolidayReports() ended at " + _timeService.GetCurrentTime());
        }

        private async Task BroadcastCoworkersAbsences()
        {
            _logger.LogInformation("BroadcastCoworkersAbsences() was initiated at " + _timeService.GetCurrentTime());
            try
            {
                var currentTime = GetCurrentDateTime();

                if (_timeService.IsFreeWorkDay(currentTime))
                {
                    var nextWorkDay = currentTime.AddDays(1);

                    while (_timeService.IsFreeWorkDay(nextWorkDay))
                    {
                        nextWorkDay = nextWorkDay.AddDays(1);
                    }

                    var allEmployees = await _employeeRepository.GetAll();
                    var allHolidays = await _holidaysRepository.GetAll();
                    var nextDayHolidays = allHolidays.Where(holiday =>
                                                                holiday.Status == HolidayStatus.Confirmed &&
                                                                holiday.FromInclusive.Date == nextWorkDay.Date
                                                            ).ToList();

                    if (nextDayHolidays.Any())
                    {
                        await _emailService.NotifyAllAboutUpcomingAbsences(allEmployees, nextDayHolidays);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in BroadcastCoworkersAbsences at " + _timeService.GetCurrentTime());
            }

            _logger.LogInformation("BroadcastCoworkersAbsences() ended at " + _timeService.GetCurrentTime());
        }

        private async Task BroadcastCoworkersBirthdays()
        {
            _logger.LogInformation("BroadcastCoworkersBirthdays() was initiated at " + _timeService.GetCurrentTime());
            try
            {
                var currentTime = GetCurrentDateTime();
                var allEmployees = await _employeeRepository.GetAll();
                var employeesWithBirthdays = allEmployees.Where(employee =>
                                                                    employee.BirthdayDate.Month == currentTime.Month &&
                                                                    employee.BirthdayDate.Day == currentTime.Day
                                                               ).ToList();

                if (employeesWithBirthdays.Any())
                {
                    await _emailService.SendBirthDayReminder(employeesWithBirthdays, allEmployees);
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in BroadcastCoworkersBirthdays() at" + _timeService.GetCurrentTime());
            }
            _logger.LogInformation("BroadcastCoworkersBirthdays() ended at " + _timeService.GetCurrentTime());
        }
    }
}
