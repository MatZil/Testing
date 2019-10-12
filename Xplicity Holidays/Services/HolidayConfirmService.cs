using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

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
                await RequestAdminApproval(holidayId, "This employee has no client that needs to confirm it");

                return true;
            }

            var client = await _repositoryClients.GetById(employee.ClientId.GetValueOrDefault());
            _emailService.ConfirmHolidayWithClient(client, employee, holiday);

            return true;
        }

        public async Task<bool> RequestAdminApproval(int holidayId, string clientStatus)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);
            var admin = await _repositoryEmployees.FindAnyAdmin();
            _emailService.ConfirmHolidayWithAdmin(admin, employee, holiday, clientStatus);

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
            updateHolidayDto.Status = "Confirmed";
            await _holidaysService.Update(holidayId, updateHolidayDto);

            if (getHolidayDto.Type == 0 && getHolidayDto.Paid)
                await UpdateEmployeesWorkdays(getHolidayDto);
        }

        private async Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto)
        {
            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToExclusive);
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            employee.FreeWorkDays -= workdays;
            await _repositoryEmployees.Update(employee);
        }
    }
}
