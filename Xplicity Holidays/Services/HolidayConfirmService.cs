using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidayConfirmService: IHolidayConfirmService
    {
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ITemplateGenerationService _templateGenerationService;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IRepository<Client> _repositoryClients;
        private readonly IRepository<Holiday> _repositoryHolidays;

        public HolidayConfirmService(IEmailService emailService, IMapper mapper, IRepository<Holiday> repositoryHolidays, 
            ITemplateGenerationService templateGenerationService, IEmployeeRepository repositoryEmployees, IRepository<Client> repositoryClients)
        {
            _emailService = emailService;
            _mapper = mapper;
            _repositoryEmployees = repositoryEmployees;
            _repositoryClients = repositoryClients;
            _repositoryHolidays = repositoryHolidays;
            _templateGenerationService = templateGenerationService;
        }
        public async Task<bool> RequestClientApproval(int holidayId)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            if (employee.ClientId == null)
            {
                await RequestAdminApproval(holidayId, "This employee has no client that needs to confirm it.");

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

        public async Task<bool> CreateRequestDocx(NewHolidayDto holidayDto, int holidayId)
        {
            var holiday = _mapper.Map<Holiday>(holidayDto);
            holiday.Id = holidayId;
            var employee = await _repositoryEmployees.GetById(holidayDto.EmployeeId);
            await _templateGenerationService.TemplateGeneration(employee.Id, holiday.Type, HolidayDocumentType.Request);

            return true;
        }

        public async Task<bool> CreateOrderDocx(int holidayId)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);
            await _templateGenerationService.TemplateGeneration(employee.Id, holiday.Type, HolidayDocumentType.Order);

            return true;
        }
    }
}
