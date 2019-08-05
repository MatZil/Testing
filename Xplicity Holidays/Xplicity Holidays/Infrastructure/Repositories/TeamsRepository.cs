using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class TeamsRepository: RepositoryBase<Team>
    {
        protected override DbSet<Team> ItemSet { get; }

        public TeamsRepository(HolidayDbContext context) : base(context)
        {
            ItemSet = context.Teams;
        }

        protected override IQueryable<Team> IncludeDependencies(IQueryable<Team> queryable)
        {
            return queryable.Include(obj => obj.Client).Include(obj => obj.Employees);
        }
    }
}
