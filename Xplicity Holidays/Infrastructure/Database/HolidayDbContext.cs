using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext: IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public HolidayDbContext(DbContextOptions options): base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            const string adminId = "53ec4767-d79f-452e-ab70-4bbe27c44fc0";
            const string roleId = "ebf2ee18-d97a-49fe-9301-e6cd31957140";
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin"
            });

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = adminId,
                UserName = "inga@xplicity.com",
                NormalizedUserName = "inga@xplicity.com",
                Email = "inga@xplicity.com",
                NormalizedEmail = "inga@xplicity.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "password"),
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
