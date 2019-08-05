using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class ClientsRepository: RepositoryBase<Client>
    {
        protected override DbSet<Client> ItemSet { get; }

        public ClientsRepository(HolidayDbContext context): base(context)
        {
            ItemSet = context.Clients;
        }

        protected override IQueryable<Client> IncludeDependencies(IQueryable<Client> queryable)
        {
            return queryable.Include(obj => obj.Team);
        }
    }
}
