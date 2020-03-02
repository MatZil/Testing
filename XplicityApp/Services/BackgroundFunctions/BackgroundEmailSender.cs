using System;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using Microsoft.Extensions.Logging;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmailService _emailService;
        private readonly IHolidayInfoService _holidayInfoService;
        private readonly ILogger<BackgroundEmailSender> _logger;

        public BackgroundEmailSender(
            ITimeService timeService, 
            IEmployeeRepository employeeRepository, 
            IHolidaysRepository holidaysRepository,
            IEmailService emailService, 
            IHolidayInfoService holidayInfoService, 
            ILogger<BackgroundEmailSender> logger)
        {
            _timeService = timeService;
            _employeeRepository = employeeRepository;
            _holidaysRepository = holidaysRepository;
            _emailService = emailService;
            _holidayInfoService = holidayInfoService;
            _logger = logger;
        }

        public async Task SendHolidayReports()
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("SendHolidayReports() was initiated at " + currentTime);

            if (currentTime.Month != currentTime.AddDays(1).Month)
            {
                try
                {
                    var allHolidays = await _holidaysRepository.GetAll();
                    var currentMonthHolidays = allHolidays.Where(h =>
                                                                    h.Status == HolidayStatus.AdminConfirmed &&
                                                                    h.FromInclusive.Year == currentTime.Year &&
                                                                    h.FromInclusive.Month == currentTime.Month
                                                                ).ToList();

                    var holidaysWithClients = await _holidayInfoService.GetClientsAndHolidays(currentMonthHolidays);
                    var admins = await _employeeRepository.GetAllAdmins();

                    await _emailService.SendThisMonthsHolidayInfo(admins, holidaysWithClients);
                }
                catch (Exception exception)
                {
                    _logger.LogInformation(exception.ToString() + " occurred in SendHolidayReports() at " + currentTime);
                }
            }

            _logger.LogInformation("SendHolidayReports() ended at " + currentTime);
        }

        public async Task BroadcastCoworkersAbsences()
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("BroadcastCoworkersAbsences() was initiated at " + currentTime);
            try
            {
                if (_timeService.IsWorkDay(currentTime))
                {
                    var nextWorkDay = _timeService.GetNextWorkDay(currentTime);

                    var allEmployees = await _employeeRepository.GetAll();
                    var allHolidays = await _holidaysRepository.GetAll();
                    var nextDayHolidays = allHolidays.Where(holiday =>
                                                                holiday.Status == HolidayStatus.AdminConfirmed &&
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
                _logger.LogInformation(exception.ToString() + " occurred in BroadcastCoworkersAbsences at " + currentTime);
            }

            _logger.LogInformation("BroadcastCoworkersAbsences() ended at " + currentTime);
        }

        public async Task BroadcastCoworkersBirthdays()
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("BroadcastCoworkersBirthdays() was initiated at " + currentTime);
            try
            {
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
                _logger.LogInformation(exception.ToString() + " occurred in BroadcastCoworkersBirthdays() at" + currentTime);
            }
            _logger.LogInformation("BroadcastCoworkersBirthdays() ended at " + currentTime);
        }
    }
}
