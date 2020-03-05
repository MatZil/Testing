using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;

namespace XplicityApp.Services.Interfaces
{
    public interface INotificationSettingsService
    {
        Task<NotificationSettingsDto> GetByEmployeeId(int employeeId);
        Task<bool> Update(int employeeId, NotificationSettingsDto notificationSettingsDto);
        Task<int> Create(int employeeId);
        Task<NotificationSettingsDto[]> GetAll();
    }
}
