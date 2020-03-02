using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<int> Create(NotificationSettings entity)
        {
            _context.NotificationSettings.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public Task<bool> Delete(NotificationSettings entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<NotificationSettings>> GetAll()
        {
            var notificationSettings = await _context.NotificationSettings.ToArrayAsync();

            return notificationSettings;
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

        public async Task<bool> Update(NotificationSettings entity)
        {
            _context.NotificationSettings.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
