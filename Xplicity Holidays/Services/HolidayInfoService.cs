using System;
using System.Collections.Generic;
using System.Linq;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidayInfoService : IHolidayInfoService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ITimeService _timeService;

        public HolidayInfoService(IEmployeeRepository repository, ITimeService timeService)
        {
            _repository = repository;
            _timeService = timeService;
        }

        public ICollection<HolidaysLeftDto> GetAllEmployeesHolidaysLeft()
        {
            var employees = _repository.GetAll().Result.ToList();

            if (employees.Count == 0)
                return null;

            var employeesHolidayCount = new List<HolidaysLeftDto>();

            foreach (var employee in employees)
            {
                var holidaysLeftDto = new HolidaysLeftDto
                {
                    EmployeeId = employee.Id,
                    HolidaysLeft = GetNumberOfHolidaysLeft(employee.Id)
                };

                employeesHolidayCount.Add(holidaysLeftDto);
            }

            return employeesHolidayCount;
        }

        public double GetNumberOfHolidaysLeft(int id)
        {
            var employee = _repository.GetById(id).Result;

            if (employee == null)
                return 0;

            var currentTime = _timeService.GetCurrentTime();
            double holidaysPerYear = employee.DaysOfVacation;
            double workDaysPerYear = _timeService.GetWorkDays(new DateTime(currentTime.Year, 1, 1)
                                                             ,new DateTime(currentTime.AddYears(1).Year, 1, 1));
            var startCheckFrom = employee.WorksFromDate;

            var workDays = _timeService.GetWorkDays(startCheckFrom, currentTime) - 1;
            var employeesHolidays = _repository.GetHolidays(employee.Id).Where(h => h.Status == "Confirmed").ToList();
            var daysOnHoliday = employeesHolidays.Sum(h => (h.ToExclusive - h.FromInclusive).Days);

            var holidaysLeft = (holidaysPerYear / workDaysPerYear) * workDays - daysOnHoliday;
            holidaysLeft = Math.Round(holidaysLeft, 2);

            return holidaysLeft;
        }
    }
}
