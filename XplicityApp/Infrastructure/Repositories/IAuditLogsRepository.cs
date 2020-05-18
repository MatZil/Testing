using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IAuditLogsRepository
    {
        Task<ICollection<AuditLog>> GetPage(int page, int pageSize);
        Task<ICollection<AuditLog>> GetPageByType(string entityType, int page, int pageSize);
        Task<int> GetItemsCountByType(string entityType);
        Task<int> GetAllItemsCount();

    }
}
