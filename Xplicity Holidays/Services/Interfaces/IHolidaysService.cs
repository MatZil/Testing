using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidaysService
    {
        Task<GetHolidayDto> GetById(int id);
        Task<ICollection<GetHolidayDto>> GetAll();
        Task<int> Create(NewHolidayDto newClient);
        Task<bool> Update(int id, UpdateHolidayDto updateData);
        Task<bool> Delete(int id);
    }
}
