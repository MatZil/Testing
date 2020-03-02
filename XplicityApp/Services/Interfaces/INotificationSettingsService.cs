using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;

namespace XplicityApp.Services.Interfaces
{
    public interface INotificationSettingsService
    {
        Task<NotificationSettingsDto> GetByEmployeeId(int employeeId);
        Task<bool> Update(int employeeId, NotificationSettingsDto updateNotificationSettingsDto);
        Task<int> Create(int employeeId);
    }
}
