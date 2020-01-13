using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task<bool> RequestAdminApproval(int holidayId, string clientStatus);
        Task ConfirmHoliday(int holidayId);
        Task<bool> IsValid(int id);
        Task<bool> IsValid(NewHolidayDto holidayDto);
        Task<bool> GenerateFilesAndNotify(int holidayId);
    }
}
