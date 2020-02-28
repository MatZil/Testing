using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        public Task<NotificationSettingsDto> GetByEmployeeId(int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<NotificationSettingsDto> Update(int employeeId, UpdateNotificationSettingsDto updateNotificationSettingsDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
