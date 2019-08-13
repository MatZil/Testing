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
    }
}
