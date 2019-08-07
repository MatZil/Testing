using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;
namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidaysService
    {
        Task<NewHolidayDto> GetById(int id);
        Task<ICollection<NewHolidayDto>> GetAll();
        Task<NewHolidayDto> Create(NewHolidayDto newClient);
        Task Update(int id, NewHolidayDto updateData);
        Task<bool> Delete(int id);
    }
}
