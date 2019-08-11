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
        private readonly IRepository<Holiday> _holidayRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailService _emailService;

        public BackgroundService(ITimeService timeService, IRepository<Holiday> holidayRepository
                               , IEmployeeRepository employeeRepository, IEmailService emailService)
        {
            _timeservice = timeService;
            _holidayRepository = holidayRepository;
            _employeeRepository = employeeRepository;
            _emailService = emailService;
        }

        public async Task RunBackgroundServices()
        {
            var currentTime = _timeservice.GetCurrentTime();
            var holidays = _holidayRepository.GetAll().Result.ToList();

            var thisMonthsHolidays = holidays.Where(h =>
                                   (h.FromInclusive.Year == currentTime.Year || h.ToExclusive.Year == currentTime.Year)
                                   && (h.FromInclusive.Month == currentTime.Month || h.ToExclusive.Month == currentTime.Month)).ToList();

            var employees = _employeeRepository.GetAll().Result.ToList();
            var admin = _employeeRepository.FindAnyAdmin().Result;

            while (true)
            {
                await Task.Run(() => CheckForLastMonthDay(admin, thisMonthsHolidays));

                await Task.Run(() => CheckUpcomingHolidays(employees, holidays));

                await Task.Delay(TimeSpan.FromHours(3));
            }
        }

        public void CheckForLastMonthDay(Employee admin, List<Holiday> holidays)
        {
            DateTime currentTIme = _timeservice.GetCurrentTime();
            DateTime nextDay = currentTIme.AddDays(1);

            if (currentTIme.Month != nextDay.Month)
            {
                //_emailService.SendThisMonthsHolidayInfo(admin, holidays);
            }
        }

        public void CheckUpcomingHolidays(List<Employee> employees, List<Holiday> holidays)
        {
            DateTime currentTime = _timeservice.GetCurrentTime();

            var upcomingHolidays = holidays.Where(holiday => holiday.IsConfirmed == true
                                && holiday.FromInclusive.ToShortDateString() == currentTime.AddDays(1).ToShortDateString())
                                .ToList();

            if (upcomingHolidays.Count != 0)
            {
                //_emailService.InformEmployeesAboutHoliday(employees, upcomingHolidays);
            }
        }


    }
}
