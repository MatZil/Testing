using AutoMapper;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.EntityBehavior;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class HolidayConfirmService: IHolidayConfirmService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHolidaysService _holidaysService;
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IRepository<Client> _repositoryClients;
        private readonly IHolidaysRepository _repositoryHolidays;
        private readonly IDocxGeneratorService _docxGeneratorService;
        public HolidayConfirmService(IEmailService emailService, IMapper mapper, IHolidaysRepository repositoryHolidays,
                                     IEmployeeRepository repositoryEmployees, IRepository<Client> repositoryClients,
                                     IHolidaysService holidaysService, ITimeService timeService, IDocxGeneratorService docxGeneratorService)
        {
            _emailService = emailService;
            _mapper = mapper;
            _repositoryEmployees = repositoryEmployees;
            _repositoryClients = repositoryClients;
            _repositoryHolidays = repositoryHolidays;
            _holidaysService = holidaysService;
            _timeService = timeService;
            _docxGeneratorService = docxGeneratorService;
        }

        public async Task<bool> RequestClientApproval(int holidayId)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            if (employee.ClientId == null)
            {
                await RequestAdminApproval(holidayId, EmployeeClientStatus.HAS_NO_CLIENT);

                return true;
            }

            var client = await _repositoryClients.GetById(employee.ClientId.GetValueOrDefault());
            await _emailService.ConfirmHolidayWithClient(client, employee, holiday);

            return true;
        }

        public async Task<bool> RequestAdminApproval(int holidayId, string clientStatus)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);
            var admins = await _repositoryEmployees.GetAllAdmins();
            await _emailService.ConfirmHolidayWithAdmin(admins, employee, holiday, clientStatus);

            return true;
        }

        public async Task ConfirmHoliday(int holidayId)
        {
            var getHolidayDto = await _holidaysService.GetById(holidayId);
            var updateHolidayDto = _mapper.Map<UpdateHolidayDto>(getHolidayDto);
            updateHolidayDto.Status = HolidayStatus.Confirmed;
            await _holidaysService.Update(holidayId, updateHolidayDto);

            if (getHolidayDto.Type == HolidayType.Parental)
            {
                await UpdateParentalLeaves(getHolidayDto);
            }
            else if (getHolidayDto.Type == HolidayType.Annual && getHolidayDto.Paid)
            {
                await UpdateEmployeesWorkdays(getHolidayDto);
                await UpdateEmployeesOvertime(getHolidayDto);
            }
        }

        private async Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto)
        {
            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            workdays -= holidayDto.OvertimeDays;
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.FreeWorkDays -= workdays;
            await _repositoryEmployees.Update(employee);
        }

        private async Task UpdateEmployeesOvertime(GetHolidayDto holidayDto)
        {
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.OvertimeHours -= holidayDto.OvertimeHours;
            await _repositoryEmployees.Update(employee);
        }

        private async Task UpdateParentalLeaves(GetHolidayDto holidayDto)
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

        public async Task ValidateHolidayConfirmationReadiness(int id)
        {
            var holiday = await _repositoryHolidays.GetById(id);

            if (holiday == null)
            {
                throw new InvalidOperationException("Holiday request not found.");
            }

            await IsValid(holiday);
        }

        public async Task ValidateNewHolidayConfirmationReadiness(NewHolidayDto holidayDto)
        {
            if (holidayDto == null)
            {
                throw new ArgumentNullException(nameof(holidayDto));
            }

            var holiday = _mapper.Map<Holiday>(holidayDto);

            await IsValid(holiday);
        }

        private async Task IsValid(Holiday holiday)
        { 
            if (holiday.Status == HolidayStatus.Confirmed)
            {
                throw new InvalidOperationException("Holiday already confirmed.");
            }

            var currentTime = _timeService.GetCurrentTime();

            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);
            
            ValidDateInterval(holiday, currentTime);
            ValidOvertime(holiday, employee);

            if (holiday.Type == HolidayType.Parental && !EmployeeEligibleForParental(holiday, employee, currentTime))
            {
                throw new InvalidOperationException("Employee is not eligible for parental leave.");
            }
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private static void ValidDateInterval(in Holiday holiday, in DateTime currentTime)
        {
            if(holiday.FromInclusive.Date <= currentTime.Date || 
                holiday.ToExclusive.Date <= holiday.FromInclusive.Date)
            {
                throw new InvalidOperationException("Requested dates for holiday are invalid.");
            }
        }

        private void ValidOvertime(Holiday holiday, Employee employee)
        {
            if (holiday.OvertimeDays <= 0) return;

            if (holiday.OvertimeDays > employee.GetOvertimeDays())
            {
                throw new InvalidOperationException("Requested holiday uses more overtime days than employee has available.");
            }

            var workdays = _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToExclusive);
            if (workdays < holiday.OvertimeDays)
                throw new InvalidOperationException("Requested holiday period doesn't have enough workdays to cover used overtime days.");
        }

        private bool EmployeeEligibleForParental(Holiday holiday, Employee employee, DateTime currentTime)
        {
            var leaveTime = _timeService.GetWorkDays(holiday.FromInclusive, holiday.ToExclusive);

            if (holiday.FromInclusive.Month != holiday.ToExclusive.AddDays(-1).Month)
            {
                var leaveTimeCurrentMonth = _timeService.GetWorkDays(holiday.FromInclusive,
                    new DateTime(holiday.FromInclusive.AddMonths(1).Year, holiday.FromInclusive.AddMonths(1).Month, 1));

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

        private async Task Notify(int fileId, int holidayId, EmployeeRoleEnum receiver)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            switch (receiver)
            {
                case EmployeeRoleEnum.Regular:
                    await _emailService.SendRequestNotification(fileId, employee.Email);
                    break;

                case EmployeeRoleEnum.Administrator:
                    var admins = await _repositoryEmployees.GetAllAdmins();
                    foreach (var admin in admins)
                    {
                        await _emailService.SendOrderNotification(fileId, employee, admin.Email); 
                    }
                    break;
            }
        }

        public async Task<bool> GenerateFilesAndNotify(int holidayId)
        {
            var fileId = await _docxGeneratorService.GenerateHolidayDocx(holidayId, FileTypeEnum.Request);
            await Notify(fileId, holidayId, EmployeeRoleEnum.Regular);

            fileId = await _docxGeneratorService.GenerateHolidayDocx(holidayId, FileTypeEnum.Order);
            await Notify(fileId, holidayId, EmployeeRoleEnum.Administrator);

            return true;
        }
    }
}
