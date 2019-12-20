using System;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Services.Extractions
{
    public class HolidayConfirmMethods
    {
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _repositoryEmployees;
        public HolidayConfirmMethods(IEmployeeRepository repositoryEmployees, ITimeService timeService)
        {
            _repositoryEmployees = repositoryEmployees;
            _timeService = timeService;
        }

        public async Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto)
        {
            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            workdays -= holidayDto.OvertimeDays;
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.FreeWorkDays -= workdays;
            await _repositoryEmployees.Update(employee);
        }

        public async Task UpdateEmployeesOvertime(GetHolidayDto holidayDto)
        {
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.OvertimeHours -= holidayDto.OvertimeHours;
            await _repositoryEmployees.Update(employee);
        }

        public async Task UpdateParentalLeaves(GetHolidayDto holidayDto)
        {
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            var leaveTime = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            var currentTime = _timeService.GetCurrentTime();

            if (holidayDto.FromInclusive.Month != holidayDto.ToExclusive.AddDays(-1).Month)
            {
                var leaveTimeCurrentMonth = _timeService.GetWorkDays(holidayDto.FromInclusive,
                                        new DateTime(holidayDto.FromInclusive.AddMonths(1).Year, holidayDto.FromInclusive.AddMonths(1).Month, 1));

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
