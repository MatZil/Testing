using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
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
