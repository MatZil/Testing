using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Extensions.Interfaces;

namespace XplicityApp.Services.Extensions
{
    public class EmployeeHolidaysConfirmationUpdater : IEmployeeHolidaysConfirmationUpdater
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IOvertimeUtility _overtimeUtility;

        public EmployeeHolidaysConfirmationUpdater(IEmployeeRepository repositoryEmployees, ITimeService timeService, IOvertimeUtility overtimeUtility)
        {
            _repositoryEmployees = repositoryEmployees;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
        }

        public async Task UpdateEmployeesOvertime(GetHolidayDto holidayDto)
        {
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            var requestedOvertimeHours = _overtimeUtility.ConvertOvertimeDaysToHours(holidayDto.OvertimeDays);
            employee.OvertimeHours -= requestedOvertimeHours;
            await _repositoryEmployees.Update(employee);
        }

        public async Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto)
        {
            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToInclusive);
            workdays -= holidayDto.OvertimeDays;
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.FreeWorkDays -= workdays;
            await _repositoryEmployees.Update(employee);
        }

        public async Task UpdateParentalLeaves(GetHolidayDto holidayDto)
        {
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            var leaveTime = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToInclusive);
            var currentTime = _timeService.GetCurrentTime();

            if (holidayDto.FromInclusive.Month != holidayDto.ToInclusive.Month)
            {
                var leaveTimeCurrentMonth = _timeService.GetRemainingMonthWorkDays(holidayDto.FromInclusive);
                var leaveTimeNextMonth = leaveTime - leaveTimeCurrentMonth;
                employee.CurrentAvailableLeaves -= leaveTimeCurrentMonth;
                employee.NextMonthAvailableLeaves -= leaveTimeNextMonth;
            }
            else if (holidayDto.FromInclusive.Month == currentTime.Month)
            {
                employee.CurrentAvailableLeaves -= leaveTime;
            }
            else
            {
                employee.NextMonthAvailableLeaves -= leaveTime;
            }

            await _repositoryEmployees.Update(employee);
        }
    }
}