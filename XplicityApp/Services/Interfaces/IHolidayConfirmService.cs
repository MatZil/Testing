using System.Threading.Tasks;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task<bool> RequestAdminApproval(int holidayId, string clientStatus);
        Task ConfirmHoliday(int holidayId);
        Task<bool> GenerateFilesAndNotify(int holidayId);
    }
}
