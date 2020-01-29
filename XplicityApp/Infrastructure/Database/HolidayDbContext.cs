using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Database
{
    public class HolidayDbContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        private readonly IConfiguration _configuration;

        public HolidayDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

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

            builder.Entity<EmailTemplate>(entity => {
                entity.HasIndex(e => e.Purpose).IsUnique();
            });
            base.OnModelCreating(builder);

            InitialDataSeeder.CreateInitialAdmin(builder, _configuration);

            InitialDataSeeder.CreateInitialEmailTemplates(builder, _configuration);

            InitialDataSeeder.CreateEquipmentCategories(builder, _configuration);
        }
    }
}
