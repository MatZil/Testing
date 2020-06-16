using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task UpdateHolidayConfirmationStatus(UpdateHolidayStatusDto updatedHolidayStatus);
        Task<HolidayGuid> GetHolidayGuid(string guid);
    }
}
