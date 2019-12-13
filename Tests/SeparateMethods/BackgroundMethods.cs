using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Tests
{
    class BackgroundMethods
    {
        public async Task AddFreeWorkDays(ICollection<Employee> employees, ITimeService _timeService, IEmployeeRepository _repository)
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

        public async Task ResetParentalLeaves(ICollection<Employee> employees, ITimeService timeService, IEmployeeRepository repository)
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
