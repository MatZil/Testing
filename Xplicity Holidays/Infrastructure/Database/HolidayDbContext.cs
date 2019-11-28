using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Static_Files;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class HolidayDbContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<FileRecord> FileRecords { get; set; }
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
            
            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = _configuration.GetValue<string>("AdminData:AdminName"),
                    Surname = _configuration.GetValue<string>("AdminData:AdminSurname"),
                    WorksFromDate = DateTime.Today,
                    BirthdayDate = DateTime.Today,
                    DaysOfVacation = 20,
                    Email = _configuration.GetValue<string>("AdminData:AdminEmail"),
                    Position = "Administrator",
                    Status = EmployeeStatusEnum.Current
                }
            );
        }
    }
}
