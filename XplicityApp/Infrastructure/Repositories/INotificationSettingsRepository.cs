using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface INotificationSettingsRepository :IRepository<NotificationSettings>
    {
        Task<NotificationSettings> GetByEmployeeId(int employeeId);
    }
}
