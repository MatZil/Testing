using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class EmployeeHolidaysBackgroundUpdater : IEmployeeHolidaysBackgroundUpdater
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeHolidaysBackgroundUpdater> _logger;
        private readonly IEmployeesService _employeesService;

        public EmployeeHolidaysBackgroundUpdater(ITimeService timeService, IEmployeeRepository employeeRepository,
                                                 ILogger<EmployeeHolidaysBackgroundUpdater> logger, IEmployeesService employeesService)
        {
            _timeService = timeService;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _employeesService = employeesService;
        }

        public async Task AddFreeWorkDays(ICollection<Employee> employees)
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("AddFreeWorkDays() was initiated at " + currentTime);
            try
            {
                if (_timeService.IsFreeWorkDay(currentTime))
                {
                    var workDaysPerYear = _timeService.GetCurrentYearWorkDays();
                    foreach (var employee in employees)
                    {
                        if (!_employeesService.HasActiveUnpaidHoliday(employee.Id))
                        {
                            employee.FreeWorkDays += Math.Round((double)employee.DaysOfVacation / workDaysPerYear, 2);
                            await _employeeRepository.Update(employee);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in AddFreeWorkDays() at " + currentTime);
            }
            _logger.LogInformation("AddFreeWorkDays() ended at " + currentTime);
        }

        public async Task ResetParentalLeaves(ICollection<Employee> employees)
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("ResetParentalLeaves() was initiated at " + currentTime);
            try
            {
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
                _logger.LogInformation(exception.ToString() + " occurred in ResetParentalLeave() at " + currentTime);
            }
            _logger.LogInformation("ResetParentalLeaves() ended at " + currentTime);
        }
    }
}
