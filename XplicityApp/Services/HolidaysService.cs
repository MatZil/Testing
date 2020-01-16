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

namespace XplicityApp.Services
{
    public class HolidaysService: IHolidaysService
    {
        private readonly IHolidaysRepository _repository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;
        private readonly IOvertimeUtility _overtimeUtility;

        public HolidaysService(IHolidaysRepository repository, IMapper mapper, ITimeService timeService, IOvertimeUtility overtimeUtility)
        {
            _repository = repository;
            _mapper = mapper;
            _timeService = timeService;
            _overtimeUtility = overtimeUtility;
        }

        public async Task<GetHolidayDto> GetById(int id)
        {
            var holiday = await _repository.GetById(id);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            return holidayDto;
        }

        public async Task<ICollection<GetHolidayDto>> GetAll()
        {
            var holidays = await _repository.GetAll();
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
            var holidayId = await _repository.Create(newHoliday);

            return holidayId;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
            {
                return false;
            }

            var deleted = await _repository.Delete(item);

            return deleted;
        }

        public async Task<bool> Update(int id, UpdateHolidayDto updateData)
        {
            if (updateData == null)
            {
                throw new ArgumentNullException(nameof(updateData));
            }

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
            {
                return false;
            }

            _mapper.Map(updateData, itemToUpdate);
            var successful = await _repository.Update(itemToUpdate);
            return successful;
        }

        public async Task<bool> Decline(int id)
        {
            var holiday = await _repository.GetById(id);

            if (holiday == null)
            {
                return false;
            }

            holiday.Status = HolidayStatus.Rejected;
            var successful = await _repository.Update(holiday);

            return successful;
        }

        public async Task<ICollection<GetHolidayDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var holidays = await _repository.GetByEmployeeStatus(employeeStatus);
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays).OrderByDescending(h => h.RequestCreatedDate).ToList();

            return holidaysDto;
        }
    }
}
