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
    public class AuditLogsRepository : RepositoryBase<AuditLog>
    {
        protected override DbSet<AuditLog> ItemSet { get; }

        public AuditLogsRepository(HolidayDbContext context) : base(context)
        {
            ItemSet = context.AuditLogs;
        }
    }
}
