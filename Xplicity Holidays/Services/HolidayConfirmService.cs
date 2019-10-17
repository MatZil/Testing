using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Infrastructure.Static_Files;
using Xplicity_Holidays.Services.Interfaces;
using System;

namespace Xplicity_Holidays.Services
{
    public class HolidayConfirmService: IHolidayConfirmService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IPdfService _pdfService;
        private readonly IHolidaysService _holidaysService;
        private readonly ITimeService _timeService;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IRepository<Client> _repositoryClients;
        private readonly IRepository<Holiday> _repositoryHolidays;

        public HolidayConfirmService(IEmailService emailService, IMapper mapper, IRepository<Holiday> repositoryHolidays, 
            IPdfService pdfService, IEmployeeRepository repositoryEmployees, IRepository<Client> repositoryClients,
            IHolidaysService holidaysService, ITimeService timeService)
        {
            _emailService = emailService;
            _mapper = mapper;
            _repositoryEmployees = repositoryEmployees;
            _repositoryClients = repositoryClients;
            _repositoryHolidays = repositoryHolidays;
            _pdfService = pdfService;
            _holidaysService = holidaysService;
            _timeService = timeService;
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
            var admin = await _repositoryEmployees.FindAnyAdmin();
            await _emailService.ConfirmHolidayWithAdmin(admin, employee, holiday, clientStatus);

            return true;
        }

        public async Task<bool> CreateRequestPdf(NewHolidayDto holidayDto, int holidayId)
        {
            var holiday = _mapper.Map<Holiday>(holidayDto);
            holiday.Id = holidayId;
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            _pdfService.CreateRequestPdf(holiday, employee);

            return true;
        }

        public async Task<bool> CreateOrderPdf(int holidayId)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);
            _pdfService.CreateOrderPdf(holiday, employee);

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
            }
        }

        private async Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto)
        {
            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.FreeWorkDays -= workdays;
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

        public async Task<bool> IsValid(object idOrDto)
        {
            Holiday holiday;

            if (idOrDto.GetType() == typeof(int))
            {
                holiday = await _repositoryHolidays.GetById((int)idOrDto);
            }
            else if (idOrDto.GetType() == typeof(NewHolidayDto))
            {
                holiday = _mapper.Map<Holiday>((NewHolidayDto)idOrDto);
            }
            else
            {
                return false;
            }

            if (holiday.Status == HolidayStatus.Confirmed)
            {
                return false;
            }

            var currentTime = _timeService.GetCurrentTime();

            if (!ValidDateInterval(holiday, currentTime))
            {
                return false;
            }

            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            if (holiday.Type == HolidayType.Parental && !EmployeeEligibleForParental(holiday, employee, currentTime))
            {
                return false;
            }

            return true;
        }

        private bool ValidDateInterval(Holiday holiday, DateTime currentTime)
        {
            if(holiday.FromInclusive.Date <= currentTime.Date || holiday.ToExclusive.Date <= holiday.FromInclusive.Date)
            {
                return false;
            }

            return true;
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
    }
}
