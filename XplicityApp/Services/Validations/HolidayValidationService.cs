using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.EntityBehavior;
using XplicityApp.Services.Validations.Interfaces;

namespace XplicityApp.Services.Validations
{
    public class HolidayValidationService : IHolidayValidationService
    {
        private readonly IHolidaysRepository _holidayRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;

        public HolidayValidationService(
            IHolidaysRepository holidayRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            ITimeService timeService)
        {
            _holidayRepository = holidayRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _timeService = timeService;
        }

        public async Task ValidateHolidayConfirmationReadiness(int holidayId, int confirmerId)
        {
            var holiday = await _holidayRepository.GetById(holidayId);

            if (holiday == null)
            {
                throw new InvalidOperationException("Holiday request not found.");
            }

            await ValidateHoliday(holiday);
            await ValidateConfirmer(confirmerId);
        }

        public async Task ValidateNewHolidayConfirmationReadiness(NewHolidayDto holidayDto)
        {
            if (holidayDto == null)
            {
                throw new ArgumentNullException(nameof(holidayDto));
            }

            var holiday = _mapper.Map<Holiday>(holidayDto);
            await ValidateHoliday(holiday);
        }

        private async Task ValidateHoliday(Holiday holiday)
        {
            if (holiday.Status == HolidayStatus.Confirmed)
            {
                throw new InvalidOperationException("Holiday already confirmed.");
            }

            var currentTime = _timeService.GetCurrentTime();
            var employee = await _employeeRepository.GetById(holiday.EmployeeId);

            ValidateDateInterval(holiday, currentTime);
            ValidateOvertime(holiday, employee);

            if (holiday.Type == HolidayType.Parental && !IsEmployeeEligibleForParental(holiday, employee, currentTime))
            {
                throw new InvalidOperationException("Employee is not eligible for parental leave.");
            }

            if (IsAnyBoundaryNotAWorkday(holiday))
            {
                throw new InvalidOperationException("Either first or last holiday's day is not a workday anyway.");
            }
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private static void ValidateDateInterval(in Holiday holiday, in DateTime currentTime)
        {
            if (holiday.FromInclusive.Date < currentTime.Date || holiday.ToInclusive.Date < holiday.FromInclusive.Date)
            {
                throw new InvalidOperationException("Requested dates for holiday are invalid.");
            }
        }

        private void ValidateOvertime(Holiday holiday, Employee employee)
        {
            if (holiday.OvertimeDays <= 0)
            {
                return;
            }

            if (holiday.OvertimeDays > employee.GetOvertimeDays())
            {
                throw new InvalidOperationException("Requested holiday uses more overtime days than employee has available.");
            }

            var workdays = _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToInclusive);
            if (workdays < holiday.OvertimeDays)
            {
                throw new InvalidOperationException("Requested holiday period doesn't have enough workdays to cover used overtime days.");
            }
        }

        private bool IsEmployeeEligibleForParental(Holiday holiday, Employee employee, DateTime currentTime)
        {
            var leaveTime = _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToInclusive);

            if (holiday.FromInclusive.Month != holiday.ToInclusive.Month)
            {
                var leaveTimeCurrentMonth = _timeService.GetRemainingMonthWorkDays(holiday.FromInclusive);
                var leaveTimeNextMonth = leaveTime - leaveTimeCurrentMonth;

                if (employee.CurrentAvailableLeaves < leaveTimeCurrentMonth || employee.NextMonthAvailableLeaves < leaveTimeNextMonth)
                {
                    return false;
                }
            }
            else if (holiday.FromInclusive.Month == currentTime.Month && employee.CurrentAvailableLeaves < leaveTime)
            {
                return false;
            }
            else if (holiday.FromInclusive.Month == currentTime.AddMonths(1).Month && employee.NextMonthAvailableLeaves < leaveTime)
            {
                return false;
            }

            return true;
        }

        private bool IsAnyBoundaryNotAWorkday(Holiday holiday)
        {
            if (!_timeService.IsWorkDay(holiday.FromInclusive) || !_timeService.IsWorkDay(holiday.ToInclusive))
            {
                return true;
            }

            return false;
        }

        private async Task ValidateConfirmer(int confirmerId)
        {
            var confirmer = await _employeeRepository.GetById(confirmerId);
            
            if (confirmer == null)
            {
                throw new InvalidOperationException("Confirmer not found.");
            }

            var admins = await _employeeRepository.GetAllAdmins();

            if (!admins.Contains(confirmer))
            {
                throw new InvalidOperationException("Confirmer is not an admin.");
            }
        }
    }
}
