using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<ICollection<InventoryItem>> GetByEmployeeId(int employeeId);

        Task<ICollection<InventoryItem>> GetByInventoryItemStatus(bool inventoryItemStatus);
    }
}
