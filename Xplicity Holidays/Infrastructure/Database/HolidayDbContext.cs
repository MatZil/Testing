using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext: DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public HolidayDbContext(DbContextOptions<HolidayDbContext> options): base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.CompanyName).IsUnique();
            });

            builder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique(); 

            });
        }
    }
}
