using System;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Extensions.Interfaces;

namespace XplicityApp.Services.Extensions
{
    public class EmployeeHolidaysBackgroundUpdater : IEmployeeHolidaysBackgroundUpdater
    {
        public async Task AddFreeWorkDays(ITimeService timeService, IEmployeeRepository employeeRepository)
        {
            var currentTime = timeService.GetCurrentTime();
            if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
            {
                var allEmployees = await employeeRepository.GetAll();
                var workDaysPerYear = timeService.GetWorkDays(new DateTime(currentTime.Year, 1, 1),
                                                            new DateTime(currentTime.AddYears(1).Year, 1, 1));
                foreach (var employee in allEmployees)
                {
                    employee.FreeWorkDays += Math.Round((double)employee.DaysOfVacation / workDaysPerYear, 2);
                    await employeeRepository.Update(employee);
                }
            }
        }

        public async Task ResetParentalLeaves(ITimeService timeService, IEmployeeRepository employeeRepository)
        {
            var currentTime = timeService.GetCurrentTime();
            if (currentTime.Day == 1)
            {
                var allEmployees = await employeeRepository.GetAll();
                foreach (var employee in allEmployees)
                {
                    employee.CurrentAvailableLeaves = employee.NextMonthAvailableLeaves;
                    employee.NextMonthAvailableLeaves = employee.ParentalLeaveLimit;
                    await employeeRepository.Update(employee);
                }
            }
        }
    }
}
