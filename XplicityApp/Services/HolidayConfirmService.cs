using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Extensions.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class HolidayConfirmService : IHolidayConfirmService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHolidaysService _holidaysService;
        private readonly IEmployeeRepository _repositoryEmployees;
        private readonly IRepository<Client> _repositoryClients;
        private readonly IHolidaysRepository _repositoryHolidays;
        private readonly IDocxGeneratorService _docxGeneratorService;
        private readonly IEmployeeHolidaysConfirmationUpdater _employeeHolidaysConfirmationUpdater;
        private readonly ILogger<HolidayConfirmService> _logger;
        private readonly IOvertimeUtility _overtimeUtility;

        public HolidayConfirmService(
            IEmailService emailService,
            IMapper mapper,
            IHolidaysRepository repositoryHolidays,
            IEmployeeRepository repositoryEmployees,
            IRepository<Client> repositoryClients,
            IHolidaysService holidaysService,
            IDocxGeneratorService docxGeneratorService,
            IOvertimeUtility overtimeUtility,
            IEmployeeHolidaysConfirmationUpdater employeeHolidaysConfirmationUpdater,
            ILogger<HolidayConfirmService> logger)
        {
            _emailService = emailService;
            _mapper = mapper;
            _repositoryEmployees = repositoryEmployees;
            _repositoryClients = repositoryClients;
            _repositoryHolidays = repositoryHolidays;
            _holidaysService = holidaysService;
            _docxGeneratorService = docxGeneratorService;
            _overtimeUtility = overtimeUtility;
            _employeeHolidaysConfirmationUpdater = employeeHolidaysConfirmationUpdater;
            _logger = logger;
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
            var overtimeSentence = _overtimeUtility.GetOvertimeSentence(OvertimeEmail.CONFIRMATION, holiday.OvertimeDays);
            await _emailService.ConfirmHolidayWithAdmin(admins, employee, holiday, clientStatus, overtimeSentence);

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
                await _employeeHolidaysConfirmationUpdater.UpdateParentalLeaves(getHolidayDto);
            }
            else if (getHolidayDto.Type == HolidayType.Annual && getHolidayDto.Paid)
            {
                await _employeeHolidaysConfirmationUpdater.UpdateEmployeesWorkdays(getHolidayDto);
                await _employeeHolidaysConfirmationUpdater.UpdateEmployeesOvertime(getHolidayDto);
            }
        }

        private async Task Notify(int fileId, int holidayId, EmployeeRoleEnum receiver)
        {
            var holiday = await _repositoryHolidays.GetById(holidayId);
            var employee = await _repositoryEmployees.GetById(holiday.EmployeeId);

            switch (receiver)
            {
                case EmployeeRoleEnum.Regular:
                    _logger.LogInformation($"About to send request notification to {employee.Email}");
                    await _emailService.SendRequestNotification(fileId, employee.Email);
                    break;

                case EmployeeRoleEnum.Administrator:
                    var admins = await _repositoryEmployees.GetAllAdmins();
                    foreach (var admin in admins)
                    {
                        _logger.LogInformation($"About to send order notification to {admin.Email}");
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
