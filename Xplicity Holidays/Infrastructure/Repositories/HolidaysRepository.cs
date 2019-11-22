using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class HolidaysRepository: RepositoryBase<Holiday>
    {
        protected override DbSet<Holiday> ItemSet { get; }

        public HolidaysRepository(HolidayDbContext context) : base(context)
        {
            ItemSet = context.Holidays;
        }
    }
}
