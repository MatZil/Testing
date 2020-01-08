using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<ICollection<InventoryItem>> GetByEmployeeId(int employeeId);
    }
}
