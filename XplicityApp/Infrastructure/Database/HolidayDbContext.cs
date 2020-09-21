using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Database
{
    public class HolidayDbContext : AuditIdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayGuid> HolidayGuids { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }
        public DbSet<InventoryCategory> InventoryCategories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<InventoryItemTag> InventoryItemsTags { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<NotificationSettings> NotificationSettings { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<BackgroundTask> BackgroundTasks { get; set; }
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

            builder.Entity<EmailTemplate>(entity =>
            {
                entity.HasIndex(e => e.Purpose).IsUnique();
            });

            builder.Entity<InventoryItemTag>().HasKey(entity =>
            new
            {
                entity.InventoryItemId,
                entity.TagId
            });

            builder.Entity<InventoryItemTag>()
                .HasOne(entity => entity.InventoryItem)
                .WithMany(e => e.InventoryItemsTags)
                .HasForeignKey(entity => entity.InventoryItemId);

            builder.Entity<InventoryItemTag>()
                .HasOne(entity => entity.Tag)
                .WithMany(e => e.InventoryItemsTags)
                .HasForeignKey(entity => entity.TagId);

            base.OnModelCreating(builder);

            InitialDataSeeder.CreateInitialAdmin(builder, _configuration);

            InitialDataSeeder.CreateInitialEmailTemplates(builder, _configuration);

            InitialDataSeeder.CreateEquipmentCategories(builder, _configuration);

            InitialDataSeeder.CreateInitialPolicyRecord(builder);

            InitialDataSeeder.CreateBackgroundTaskLog(builder);
        }
    }
}
