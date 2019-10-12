using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext : IdentityDbContext<User>
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

            builder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.CompanyName).IsUnique();
            });

            builder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            base.OnModelCreating(builder);

            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Inga",
                    Surname = "Rana",
                    WorksFromDate = DateTime.Today,
                    BirthdayDate = DateTime.Today,
                    DaysOfVacation = 20,
                    Email = "Inga@xplicity.com",
                    Position = "Position"
                }
            );
        }
    }
}
