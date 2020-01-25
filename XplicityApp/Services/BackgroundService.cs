using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Extensions.Interfaces;
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

        public BackgroundService(ITimeService timeService, IEmployeeHolidaysBackgroundUpdater employeeHolidaysBackgroundUpdater,
                            IEmployeeRepository employeeRepository, IHolidaysRepository holidaysRepository,
                            IEmailService emailService, IHolidayInfoService holidayInfoService,
                            IWebHostEnvironment webHostEnvironment)
        {
            _timeService = timeService;
            _employeeHolidaysBackgroundUpdater = employeeHolidaysBackgroundUpdater;
            _employeeRepository = employeeRepository;
            _holidaysRepository = holidaysRepository;
            _emailService = emailService;
            _holidayInfoService = holidayInfoService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DoBackgroundTasks()
        {
            await SendHolidayReports();

            await BroadcastCoworkersAbsences();

            await BroadcastCoworkersBirthdays();

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(_timeService, _employeeRepository);

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(_timeService, _employeeRepository);
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
            var currentTime = GetCurrentDateTime();

            if (currentTime.Month != currentTime.AddDays(1).Month)
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
        }

        private async Task BroadcastCoworkersAbsences()
        {
            var currentTime = GetCurrentDateTime();
            var allEmployees = await _employeeRepository.GetAll();
            var allHolidays = await _holidaysRepository.GetAll();
            var nextDayHolidays = allHolidays.Where(holiday =>
                                                        holiday.Status == HolidayStatus.Confirmed &&
                                                        holiday.FromInclusive.Date == currentTime.AddDays(1).Date
                                                    ).ToList();

            if (nextDayHolidays.Count > 0)
            {
                await _emailService.NotifyAllAboutUpcomingAbsences(allEmployees, nextDayHolidays);
            }
        }

        private async Task BroadcastCoworkersBirthdays()
        {
            var currentTime = GetCurrentDateTime();
            var allEmployees = await _employeeRepository.GetAll();
            var employeesWithBirthdays = allEmployees.Where(employee =>
                                                                employee.BirthdayDate.Month == currentTime.Month &&
                                                                employee.BirthdayDate.Day == currentTime.Day
                                                           ).ToList();

            if (employeesWithBirthdays.Count > 0)
            {
                await _emailService.SendBirthDayReminder(employeesWithBirthdays, allEmployees);
            }
        }
    }
}
