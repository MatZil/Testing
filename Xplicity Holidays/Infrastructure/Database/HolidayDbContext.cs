using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext : IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        private readonly IConfiguration _configuration;

        public HolidayDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            string adminId = _configuration["AdminConfig:AdminGuid"];
            string roleId = _configuration["AdminConfig:RoleGuid"];
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = _configuration["AdminConfig:RoleName"],
                NormalizedName = _configuration["AdminConfig:RoleName"]
            });

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = adminId,
                UserName = _configuration["AdminConfig:AdminEmail"],
                NormalizedUserName = _configuration["AdminConfig:AdminEmail"],
                Email = _configuration["AdminConfig:AdminEmail"],
                NormalizedEmail = _configuration["AdminConfig:AdminEmail"],
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, _configuration["AdminConfig:AdminPassword"]),
                SecurityStamp = string.Empty
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = adminId
            });
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
