using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace XplicityApp.Infrastructure.Repositories
{
    public class AuditLogsRepository : IAuditLogsRepository
    {
        protected readonly HolidayDbContext Context;

        public async Task<int> Create(AuditLog entity)
        {
            Context.AuditLogs.Add(entity);
            await Context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(AuditLog entity)
        {
            Context.AuditLogs.Remove(entity);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<ICollection<AuditLog>> GetAll()
        {
            var auditLogs = await Context.AuditLogs.ToArrayAsync();

            return auditLogs;
        }

        public async Task<AuditLog> GetById(int id)
        {
            var auditLog = await Context.AuditLogs.FindAsync(id);

            return auditLog;
        }

        public async Task<bool> Update(AuditLog entity)
        {
            Context.AuditLogs.Attach(entity);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
