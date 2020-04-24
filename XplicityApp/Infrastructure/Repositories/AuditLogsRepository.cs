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

        public async Task<ICollection<AuditLog>> GetAll()
        {
            var auditLogs = await _context.AuditLogs.ToArrayAsync();

            return auditLogs;
        }

        public async Task<ICollection<AuditLog>> GetPage(int page, int pageSize)
        {
            var auditLogs = await _context.AuditLogs.Skip(page * pageSize).Take(pageSize).ToArrayAsync();

            return auditLogs;
        }

        public async Task<ICollection<AuditLog>> GetByEntityType(string entityType, int page, int pageSize)
        {
            var auditLogs = await _context.AuditLogs.Where(x => x.EntityType == entityType).
                Skip(page * pageSize).Take(pageSize).ToArrayAsync();

            return auditLogs;
        }
    }
}
