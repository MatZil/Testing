using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Static_Files;

namespace XplicityApp.Infrastructure.Database
{
    public static class InitialDataSeeder
    {
        public static void CreateInitialAdmin(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = configuration["AdminData:AdminName"],
                    Surname = configuration["AdminData:AdminSurname"],
                    WorksFromDate = DateTime.MinValue,
                    BirthdayDate = DateTime.MinValue,
                    DaysOfVacation = 20,
                    Email = configuration["AdminData:AdminEmail"],
                    Position = "Administrator",
                    Status = EmployeeStatusEnum.Current
                }
            );
        }

        public static void CreateInitialPolicyRecord(ModelBuilder builder)
        {
            var guid = Guid.NewGuid().ToString();
            builder.Entity<FileRecord>().HasData(
                new FileRecord
                {
                    Guid = guid,
                    Id = 1,
                    Name = "Holiday Policy.pdf",
                    Type = FileTypeEnum.HolidayPolicy,
                    CreatedAt = DateTime.MinValue
                });
        }

        public static void CreateInitialEmailTemplates(ModelBuilder builder, IConfiguration configuration)
        {
            CreateAdminConfirmation(builder, configuration);
            CreateClientConfirmation(builder, configuration);
            CreateMonthlyReport(builder, configuration);
            CreateBirthdayReminder(builder, configuration);
            CreateHolidayNotification(builder, configuration);
            CreateOrderNotification(builder, configuration);
            CreateRejectedRequestNotification(builder, configuration);
            CreateRequestNotification(builder, configuration);
        }

        private static void CreateAdminConfirmation(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 1,
                    Purpose = EmailPurposes.ADMIN_CONFIRMATION,
                    Subject = configuration["EmailTemplates:AdminConfirmation:Subject"],
                    Template = configuration["EmailTemplates:AdminConfirmation:Template"],
                    Instructions = configuration["EmailTemplates:AdminConfirmation:Instructions"]
                }
            );
        }

        private static void CreateClientConfirmation(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 2,
                    Purpose = EmailPurposes.CLIENT_CONFIRMATION,
                    Subject = configuration["EmailTemplates:ClientConfirmation:Subject"],
                    Template = configuration["EmailTemplates:ClientConfirmation:Template"],
                    Instructions = configuration["EmailTemplates:ClientConfirmation:Instructions"]
                }
            );
        }

        private static void CreateMonthlyReport(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 3,
                    Purpose = EmailPurposes.MONTHLY_HOLIDAYS_REPORT,
                    Subject = configuration["EmailTemplates:MonthlyReport:Subject"],
                    Template = configuration["EmailTemplates:MonthlyReport:Template"],
                    Instructions = configuration["EmailTemplates:MonthlyReport:Instructions"]
                }
            );
        }

        private static void CreateHolidayNotification(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 4,
                    Purpose = EmailPurposes.HOLIDAY_REMINDER,
                    Subject = configuration["EmailTemplates:HolidayNotification:Subject"],
                    Template = configuration["EmailTemplates:HolidayNotification:Template"],
                    Instructions = configuration["EmailTemplates:HolidayNotification:Instructions"]
                }
            );
        }

        private static void CreateBirthdayReminder(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 5,
                    Purpose = EmailPurposes.BIRTHDAY_REMINDER,
                    Subject = configuration["EmailTemplates:BirthdayReminder:Subject"],
                    Template = configuration["EmailTemplates:BirthdayReminder:Template"],
                    Instructions = configuration["EmailTemplates:BirthdayReminder:Instructions"]
                }
            );
        }

        private static void CreateRequestNotification(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 6,
                    Purpose = EmailPurposes.REQUEST_NOTIFICATION,
                    Subject = configuration["EmailTemplates:RequestNotification:Subject"],
                    Template = configuration["EmailTemplates:RequestNotification:Template"],
                    Instructions = configuration["EmailTemplates:RequestNotification:Instructions"]
                }
            );
        }

        private static void CreateOrderNotification(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 7,
                    Purpose = EmailPurposes.ORDER_NOTIFICATION,
                    Subject = configuration["EmailTemplates:OrderNotification:Subject"],
                    Template = configuration["EmailTemplates:OrderNotification:Template"],
                    Instructions = configuration["EmailTemplates:OrderNotification:Instructions"]
                }
            );
        }

        private static void CreateRejectedRequestNotification(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<EmailTemplate>().HasData(
                new EmailTemplate
                {
                    Id = 8,
                    Purpose = EmailPurposes.REJECTION_NOTIFICATION,
                    Subject = configuration["EmailTemplates:RejectionNotification:Subject"],
                    Template = configuration["EmailTemplates:RejectionNotification:Template"],
                    Instructions = configuration["EmailTemplates:RejectionNotification:Instructions"]
                }
            );
        }

        public static void CreateEquipmentCategories(ModelBuilder builder, IConfiguration configuration)
        {
            builder.Entity<InventoryCategory>().HasData(
                new InventoryCategory
                {
                    Id = 1,
                    Name = configuration["EquipmentCategories:Furniture:Name"],
                    Deprecation = Convert.ToInt32(configuration["EquipmentCategories:Furniture:Deprecation"])
                },
                new InventoryCategory
                {
                    Id = 2,
                    Name = configuration["EquipmentCategories:Electronics:Name"],
                    Deprecation = Convert.ToInt32(configuration["EquipmentCategories:Electronics:Deprecation"])
                },
                new InventoryCategory
                {
                    Id = 3,
                    Name = configuration["EquipmentCategories:Other:Name"],
                    Deprecation = Convert.ToInt32(configuration["EquipmentCategories:Other:Deprecation"])
                },
                new InventoryCategory
                {
                    Id = 4,
                    Name = configuration["EquipmentCategories:License:Name"],
                    Deprecation = Convert.ToInt32(configuration["EquipmentCategories:License:Deprecation"])
                }
            );
        }

        public static void CreateBackgroundTaskLog(ModelBuilder builder)
        {
            builder.Entity<BackgroundTask>().HasData(
                new BackgroundTask
                {
                    Id = 1,
                    ExecutionDate = DateTime.Today
                });
        }
    }
}
