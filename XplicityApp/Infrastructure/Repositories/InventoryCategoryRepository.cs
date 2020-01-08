using Microsoft.EntityFrameworkCore;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class InventoryCategoryRepository : RepositoryBase<InventoryCategory>
    {
        protected override DbSet<InventoryCategory> ItemSet { get; }

        public InventoryCategoryRepository(HolidayDbContext context) : base(context)
        {
            ItemSet = context.InventoryCategories;
        }
    }
}
