using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IInventoryItemTagsRepository : IRepository<InventoryItemTag>
    {
        Task<InventoryItemTag> FindByInventoryItemIdAndTagId(int itemId, int tagId);
        Task<ICollection<InventoryItemTag>> FindAllByInventoryItemId(int itemId);
    }
}
