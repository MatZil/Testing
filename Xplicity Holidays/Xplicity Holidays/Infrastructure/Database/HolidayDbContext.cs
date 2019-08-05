using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext: DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Team> Teams { get; set; }

        public HolidayDbContext(DbContextOptions<HolidayDbContext> options): base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUpClients(modelBuilder);
        }

        private void SetUpClients(ModelBuilder modelBuilder)
        {
            var clientEntity = modelBuilder.Entity<Client>();
            clientEntity.HasKey(col => col.Id);
            clientEntity.HasOne(obj => obj.Team)
                .WithOne(obj => obj.Client)
                .HasForeignKey<Client>(obj => obj.Id);
        }


        
    }
}
