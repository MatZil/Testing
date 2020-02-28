using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class NotificationSettingsRepository : INotificationSettingsRepository
    {
        private readonly HolidayDbContext _context;

        public NotificationSettingsRepository(HolidayDbContext context)
        {
            _context = context;
        }
        public Task<int> Create(NotificationSettings entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(NotificationSettings entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<NotificationSettings>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<NotificationSettings> GetByEmployeeId(int employeeId)
        {
            var notificationSettings = await _context.NotificationSettings.Where(setting => setting.EmployeeId == employeeId).FirstOrDefaultAsync();

            return notificationSettings;
        }

        public Task<NotificationSettings> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(NotificationSettings entity)
        {
            throw new NotImplementedException();
        }
    }
}
