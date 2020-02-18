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

namespace XplicityApp.Services
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHolidaysRepository _holidaysRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly IOvertimeUtility _overtimeUtility;

        public HolidaysService(IHolidaysRepository holidaysRepository, IEmployeeRepository employeeRepository, IMapper mapper, ITimeService timeService, IOvertimeUtility overtimeUtility)
        {
            _holidaysRepository = holidaysRepository;
            _mapper = mapper;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
            _employeeRepository = employeeRepository;
        }

        public async Task<GetHolidayDto> GetById(int id)
        {
            var holiday = await _holidaysRepository.GetById(id);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            return holidayDto;
        }

        public async Task<ICollection<GetHolidayDto>> GetAll()
        {
            var holidays = await _holidaysRepository.GetAll();
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();

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

        public async Task<bool> Decline(int holidayId, int confirmerId)
        {
            var getHolidayDto = await _holidaysRepository.GetById(holidayId);
            var confirmer = await _employeeRepository.GetById(confirmerId);

            if (getHolidayDto == null || confirmer == null)
            {
                return false;
            }

            var updateHolidayDto = _mapper.Map<UpdateHolidayDto>(getHolidayDto);
            updateHolidayDto.Status = HolidayStatus.Rejected;
            updateHolidayDto.ConfirmerFullName = $"{confirmer.Name} {confirmer.Surname}";
            var successful = await Update(holidayId, updateHolidayDto);

            return successful;
        }

        public async Task<ICollection<GetHolidayDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var holidays = await _holidaysRepository.GetByEmployeeStatus(employeeStatus);
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();
            holidaysDto.ForEach(holiday => AddOvertimeDetails(holiday));

            return holidaysDto;
        }

        private GetHolidayDto AddOvertimeDetails(GetHolidayDto holiday)
        {
            return _overtimeUtility.AddOvertimeDetailsToHoliday(holiday);
        }
    }
}
