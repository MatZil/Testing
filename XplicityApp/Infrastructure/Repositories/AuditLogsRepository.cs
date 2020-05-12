using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace XplicityApp.Infrastructure.Repositories
{
    public class AuditLogsRepository : IAuditLogsRepository
    {
        protected readonly HolidayDbContext _context;

        public AuditLogsRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<AuditLog>> GetPage(int page, int pageSize)
        {
            var auditLogs = await _context.AuditLogs.Skip(page * pageSize).Take(pageSize).ToArrayAsync();

            return auditLogs;
        }

        public async Task<ICollection<AuditLog>> GetPageByType(string entityType, int page, int pageSize)
        {
            var auditLogs = await _context.AuditLogs.Where(x => x.EntityType == entityType).
                Skip(page * pageSize).Take(pageSize).ToArrayAsync();

            return auditLogs;
        }

        public async Task<int> GetItemsCountByType(string entityType)
        {
            var logsCount = await _context.AuditLogs.Where(x => x.EntityType == entityType).CountAsync();

            return logsCount;
        }

        public async Task<int> GetAllItemsCount()
        {
            var logsCount = await _context.AuditLogs.CountAsync();

            return logsCount;
        }
    }
}
