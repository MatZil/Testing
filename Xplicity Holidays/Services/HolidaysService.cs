using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidaysService: IHolidaysService
    {
        private readonly IRepository<Holiday> _repository;
        private readonly IMapper _mapper;
        private readonly ITimeService _timeService;

        public HolidaysService(IRepository<Holiday> repository, IMapper mapper, ITimeService timeService)
        {
            _repository = repository;
            _mapper = mapper;
            _timeService = timeService;
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
            var holidaysDto = _mapper.Map<GetHolidayDto[]>(holidays);

            return holidaysDto;
        }

        public async Task<int> Create(NewHolidayDto newHolidayDto)
        {
            if (newHolidayDto == null)
            {
                return -1;
            }

            var newHoliday = _mapper.Map<Holiday>(newHolidayDto);
            newHoliday.RequestCreatedDate = _timeService.GetCurrentTime();
            newHoliday.Status = HolidayStatus.Unconfirmed;
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
                return false;
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
    }
}
