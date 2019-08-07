using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class HolidaysService: IHolidaysService
    {
        private readonly IRepository<Holiday> _repository;
        private readonly IMapper _mapper;

        public HolidaysService(IRepository<Holiday> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NewHolidayDto> GetById(int id)
        {
            var holiday = await _repository.GetById(id);
            var holidayDto = _mapper.Map<NewHolidayDto>(holiday);
            return holidayDto;
        }

        public async Task<ICollection<NewHolidayDto>> GetAll()
        {
            var holidays = await _repository.GetAll();
            var holidaysDto = _mapper.Map<NewHolidayDto[]>(holidays);
            return holidaysDto;
        }

        public async Task<NewHolidayDto> Create(NewHolidayDto newHolidayDto)
        {
            if (newHolidayDto == null) throw new ArgumentNullException(nameof(newHolidayDto));

            var newHoliday = _mapper.Map<Holiday>(newHolidayDto);
            await _repository.Create(newHoliday);

            var holidayDto = _mapper.Map<NewHolidayDto>(newHoliday);
            return holidayDto;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);
            if (item == null)
                return false;

            var deleted = await _repository.Delete(item);
            return deleted;
        }

        public async Task Update(int id, NewHolidayDto updateData)
        {
            if (updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var itemToUpdate = await _repository.GetById(id);

            if (itemToUpdate == null)
                throw new InvalidOperationException();

            _mapper.Map(updateData, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }
    }
}
