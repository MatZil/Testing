using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task<bool> RequestAdminApproval(int holidayId, string clientStatus, int? confirmerId);
        Task ConfirmHoliday(int holidayId, int confirmerId);
        Task<bool> GenerateFilesAndNotify(int holidayId);
        Task UpdateHolidayConfirmationStatus(UpdateHolidayStatusDto updatedHolidayStatus);
    }
}
