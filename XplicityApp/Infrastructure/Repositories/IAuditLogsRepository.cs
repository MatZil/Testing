using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IAuditLogsRepository
    {
        Task<ICollection<AuditLog>> GetAll();
        Task<ICollection<AuditLog>> GetPage(int page, int pageSize);
        Task<ICollection<AuditLog>> GetByEntityType(string entityType, int page, int pageSize);
    }
}
