using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task UpdateHolidayConfirmationStatus(UpdateHolidayStatusDto updatedHolidayStatus);
    }
}
