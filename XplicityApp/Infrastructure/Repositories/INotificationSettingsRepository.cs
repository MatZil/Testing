using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface INotificationSettingsRepository :IRepository<NotificationSettings>
    {
        Task<NotificationSettings> GetByEmployeeId(int employeeId);
    }
}
