using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;

namespace XplicityApp.Services.Interfaces
{
    public interface INotificationSettingsService
    {
        Task<NotificationSettingsDto> GetByEmployeeId(int employeeId);
        Task<NotificationSettingsDto> Update(int employeeId, UpdateNotificationSettingsDto updateNotificationSettingsDto);
    }
}
