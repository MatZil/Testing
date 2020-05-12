using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Dtos.AuditLogs
{
    public class GetAuditLogsDto
    {
        public ICollection<AuditLog> Logs { get; set; }
        public int TotalCount { get; set; }
    }
}
