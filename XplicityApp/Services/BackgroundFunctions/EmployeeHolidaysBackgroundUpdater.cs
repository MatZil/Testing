using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class EmployeeHolidaysBackgroundUpdater : IEmployeeHolidaysBackgroundUpdater
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeHolidaysBackgroundUpdater> _logger;

        public EmployeeHolidaysBackgroundUpdater(ITimeService timeService, IEmployeeRepository employeeRepository, 
                                                 ILogger<EmployeeHolidaysBackgroundUpdater> logger)
        {
            _timeService = timeService;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task AddFreeWorkDays(ICollection<Employee> employees)
        {
            _logger.LogInformation("AddFreeWorkDays() was initiated at " + _timeService.GetCurrentTime());
            try
            {
                var currentTime = _timeService.GetCurrentTime();
                if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
                {
                    var workDaysPerYear = _timeService.GetWorkDays(new DateTime(currentTime.Year, 1, 1),
                                                                new DateTime(currentTime.AddYears(1).Year, 1, 1));
                    foreach (var employee in employees)
                    {
                        employee.FreeWorkDays += Math.Round((double)employee.DaysOfVacation / workDaysPerYear, 2);
                        await _employeeRepository.Update(employee);
                    }
                }
            } 
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in AddFreeWorkDays() at " + _timeService.GetCurrentTime());
            }
            _logger.LogInformation("AddFreeWorkDays() ended at " + _timeService.GetCurrentTime());
        }

        public async Task ResetParentalLeaves(ICollection<Employee> employees)
        {
            _logger.LogInformation("ResetParentalLeaves() was initiated at " + _timeService.GetCurrentTime());
            try
            {
                var currentTime = _timeService.GetCurrentTime();
                if (currentTime.Day == 1)
                {
                    foreach (var employee in employees)
                    {
                        employee.CurrentAvailableLeaves = employee.NextMonthAvailableLeaves;
                        employee.NextMonthAvailableLeaves = employee.ParentalLeaveLimit;
                        await _employeeRepository.Update(employee);
                    }
                }
            } 
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in ResetParentalLeave() at " + _timeService.GetCurrentTime());
            }
            _logger.LogInformation("ResetParentalLeaves() ended at " + _timeService.GetCurrentTime());
            
        }
    }
}
