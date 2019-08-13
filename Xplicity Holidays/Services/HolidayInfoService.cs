﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
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

        public List<(string, double)> GetAllEmployeesHolidaysLeft()
        {
            List<Employee> employees = _repository.GetAll().Result.ToList();

            if (employees.Count == 0)
                return null;

            List<(string, double)> holidayDays = new List<(string, double)>();

            foreach (var employee in employees)
            {
                double holidaysLeft = GetNumberOfHolidaysLeft(employee.Id);
                holidayDays.Add((employee.Name + " " + employee.Surname, holidaysLeft));
            }

            return holidayDays;
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
            DateTime startCheckFrom = employee.WorksFromDate;

            int workDays = _timeService.GetWorkDays(startCheckFrom, currentTime) - 1;
            List<Holiday> employeesHolidays = _repository.GetHolidays(employee.Id).Where(h => h.IsConfirmed == true).ToList();
            int daysOnHoliday = employeesHolidays.Sum(h => (h.ToExclusive - h.FromInclusive).Days);

            double holidaysLeft = (holidaysPerYear / workDaysPerYear) * workDays - daysOnHoliday;
            holidaysLeft = Math.Round(holidaysLeft, 2);
            return holidaysLeft;
        }
    }
}
