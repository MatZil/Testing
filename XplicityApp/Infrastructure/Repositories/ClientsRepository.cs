using Microsoft.EntityFrameworkCore;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
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
