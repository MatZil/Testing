using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace XplicityApp.Services
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepository<Client> _clientsRepository;
        private readonly IHolidayGuidsRepository _holidayGuidsRepository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly IOvertimeUtility _overtimeUtility;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public HolidaysService(
            IHolidaysRepository holidaysRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            ITimeService timeService,
            IOvertimeUtility overtimeUtility,
            IRepository<Client> clientsRepository,
            IUserService userService,
            IConfiguration configuration,
            IHolidayGuidsRepository holidayGuidsRepository
            )
        {
            _holidaysRepository = holidaysRepository;
            _mapper = mapper;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
            _employeeRepository = employeeRepository;
            _clientsRepository = clientsRepository;
            _userService = userService;
            _configuration = configuration;
            _holidayGuidsRepository = holidayGuidsRepository;
        }

        public async Task<GetHolidayDto> GetById(int id)
        {
            var holiday = await _holidaysRepository.GetById(id);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            if (holidayDto != null)
            {
                holidayDto.ConfirmerFullName = await GetConfirmerFullName(holidayDto);
            }

            return holidayDto;
        }

        public async Task<ICollection<GetHolidayDto>> GetAll()
        {
            var holidays = await _holidaysRepository.GetAll();
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();

            foreach (var holidayDto in holidaysDto)
            {
                holidayDto.ConfirmerFullName = await GetConfirmerFullName(holidayDto);
            }

            return holidaysDto;
        }

        public async Task<int> Create(NewHolidayDto newHolidayDto)
        {
            if (newHolidayDto == null)
            {
                throw new ArgumentNullException();
            }

            var newHoliday = _mapper.Map<Holiday>(newHolidayDto);
            newHoliday.RequestCreatedDate = _timeService.GetCurrentTime();
            newHoliday.Status = HolidayStatus.Pending;
            var holidayId = await _holidaysRepository.Create(newHoliday);

            var employee = await _employeeRepository.GetById(newHoliday.EmployeeId);
            if (employee.ClientId > 0)
            {
                var newHolidayGuid = new HolidayGuid()
                {
                    IsAdmin = false,
                    ConfirmerId = employee.ClientId,
                    HolidayId = holidayId,
                    Guid = Guid.NewGuid().ToString()
                };
                await _holidayGuidsRepository.Create(newHolidayGuid);
            }

            var allAdmins = await _employeeRepository.GetAllAdmins();
            foreach (var admin in allAdmins)
            {
                var newHolidayGuid = new HolidayGuid()
                {
                    IsAdmin = true,
                    ConfirmerId = admin.Id,
                    HolidayId = holidayId,
                    Guid = Guid.NewGuid().ToString()
                };
                await _holidayGuidsRepository.Create(newHolidayGuid);
            }

            return holidayId;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _holidaysRepository.GetById(id);

            if (item == null)
            {
                return false;
            }

            var deleted = await _holidaysRepository.Delete(item);

            return deleted;
        }

        public async Task<bool> Update(int id, UpdateHolidayDto updatedHoliday)
        {
            if (updatedHoliday is null)
            {
                throw new ArgumentNullException(nameof(updatedHoliday));
            }

            var itemToUpdate = await _holidaysRepository.GetById(id);
            if (itemToUpdate is null)
            {
                return false;
            }

            _mapper.Map(updatedHoliday, itemToUpdate);
            return await _holidaysRepository.Update(itemToUpdate);
        }

        public async Task<ICollection<GetHolidayDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var holidays = await _holidaysRepository.GetByEmployeeStatus(employeeStatus);
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();

            holidaysDto.ForEach(holidayDto => AddOvertimeDetails(holidayDto));
            foreach (var holidayDto in holidaysDto)
            {
                holidayDto.ConfirmerFullName = await GetConfirmerFullName(holidayDto);
            }

            return holidaysDto;
        }

        private GetHolidayDto AddOvertimeDetails(GetHolidayDto holiday)
        {
            return _overtimeUtility.AddOvertimeDetailsToHoliday(holiday);
        }

        public async Task<string> GetEmployeeFullName(int employeeId)
        {
            var employee = await _employeeRepository.GetById(employeeId);
            var employeeFullName = $"{employee.Name} {employee.Surname}";
            return employeeFullName;
        }

        public async Task<string> GetClientFullName(int clientId)
        {
            var client = await _clientsRepository.GetById(clientId);
            var clientFullName = client.CompanyName;
            return clientFullName;
        }

        public async Task<string> GetConfirmerFullName(GetHolidayDto holidayDto)
        {
            var confirmerFullName = string.Empty;
            if (holidayDto.ConfirmerAdminId > 0)
            {
                confirmerFullName = await GetEmployeeFullName(holidayDto.ConfirmerAdminId);
            }
            else if (holidayDto.ConfirmerClientId > 0)
            {
                confirmerFullName = await GetClientFullName(holidayDto.ConfirmerClientId);
            }

            return confirmerFullName;
        }
        public async Task<List<GetHolidayDto>> GetFilteredConfirmedByMonth(DateTime selectedDate, int currentUserId, int filter)
        {
            var dateFrom = _timeService.GetCalendarDateFrom(_configuration, selectedDate);
            var dateTo = _timeService.GetCalendarDateTo(_configuration, selectedDate);

            var holidays = await GetByRole(currentUserId);
            var selectedMonthConfirmedHolidays = new List<GetHolidayDto>();

            foreach (var holiday in holidays)
            {
                if (holiday.Status == HolidayStatus.AdminConfirmed)
                {
                    bool datesOverlap = dateFrom < holiday.ToInclusive && holiday.FromInclusive <= dateTo;
                    if (datesOverlap)
                    {
                        if (filter == -1 && holiday.EmployeeId == currentUserId)
                        {
                            selectedMonthConfirmedHolidays.Add(holiday);
                        }
                        else if (filter > 0 && filter == holiday.ConfirmerClientId)
                        {
                            selectedMonthConfirmedHolidays.Add(holiday);
                        }
                        else if (filter == 0)
                            selectedMonthConfirmedHolidays.Add(holiday);
                    }
                }
            }
            return selectedMonthConfirmedHolidays;
        }

        private async Task<ICollection<GetHolidayDto>> GetByRole(int currentUserId)
        {
            var employee = await _employeeRepository.GetById(currentUserId);
            var holidays = await _holidaysRepository.GetAll();
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();

            var holidaysFinal = new List<GetHolidayDto>();
            var employeeRole = await _userService.GetUserRole(currentUserId);

            if (employeeRole == "Admin")
            {
                foreach (var holidayDto in holidaysDto)
                {
                    holidayDto.EmployeeFullName = await GetEmployeeFullName(holidayDto.EmployeeId);
                    holidaysFinal.Add(holidayDto);
                }
            }
            else
            {
                foreach (var holidayDto in holidaysDto)
                {
                    if (holidayDto.EmployeeId == currentUserId || holidayDto.ConfirmerClientId == employee.ClientId)
                    {
                        holidaysFinal.Add(holidayDto);
                        holidayDto.EmployeeFullName = await GetEmployeeFullName(holidayDto.EmployeeId);
                    }
                }
            }

            return holidaysFinal;
        }

        public async Task<string> GetConfirmationLink(int holidayId, int confirmerId, bool isAdmin)
        {
            var holidayGuid = await _holidayGuidsRepository.GetGuid(holidayId, confirmerId, isAdmin);
            return $"{_configuration["AppSettings:RootUrl"]}/HolidayConfirmation?request={holidayGuid}";
        }
    }
}
