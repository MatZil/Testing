﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XplicityApp.Infrastructure.Database;

namespace XplicityApp.Migrations
{
    [DbContext(typeof(HolidayDbContext))]
    [Migration("20200219145132_EmailTemplateChange")]
    partial class EmailTemplateChange
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(6000);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("OwnerEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<string>("OwnerPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<string>("OwnerSurname")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.EmailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Purpose")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Purpose")
                        .IsUnique();

                    b.ToTable("EmailTemplates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Instructions = @"{admin.name} - Receiving admin's first name.
{employee.fullName} - Employee's full name.
{holiday.paid} - Whether or not holiday is paid.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{client.status} - Message that client has confirmed the holiday.
{holiday.overtimeHours} - Message that employee wants to use some of his overtime hours for this holiday.
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.",
                            Purpose = "Admin Confirmation",
                            Subject = "An employee is requesting confirmation for holidays",
                            Template = @"Hello, {admin.name},

An employee {employee.fullName} is intending to go on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}"
                        },
                        new
                        {
                            Id = 2,
                            Instructions = @"{client.name} - Receiving client's first name.
 {employee.fullName} - Employee's full name.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.",
                            Purpose = "Client Confirmation",
                            Subject = "One of your employees is requesting confirmation for holidays",
                            Template = @"Hello, {client.name},

An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive).

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}"
                        },
                        new
                        {
                            Id = 3,
                            Instructions = @"{client.name} - Individual team's client's name.
{employee.fullName} - Individual employee's full name.
{holiday.paid} - Whether or not the holiday is paid.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's beginning date.
{holiday.overtimeHours} - Message about individual employee's overtime hours.

Please use the first line for team's title, second line for individual employee's info.",
                            Purpose = "Monthly Holidays' Report",
                            Subject = "Monthly Holidays' Report Grouped By Teams",
                            Template = @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}"
                        },
                        new
                        {
                            Id = 5,
                            Instructions = "{employee.fullName} - Employee's full name.",
                            Purpose = "Birthday Reminder",
                            Subject = "One of your colleagues is having their birthday today!",
                            Template = "Your colleague {employee.fullName} is having their birthday today! Don't forget to congratulate them."
                        },
                        new
                        {
                            Id = 4,
                            Instructions = @"{employee.fullName} - Employee's full name.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.",
                            Purpose = "Upcoming Holiday Reminder",
                            Subject = "One of your colleagues is going away for holidays next workday!",
                            Template = "Your colleague {employee.fullName} is going away for holidays next workday from {holiday.from} to {holiday.to} (inclusive)."
                        },
                        new
                        {
                            Id = 7,
                            Instructions = @"{employee.fullName} - Employee's full name.
{download.link} - A link to download order document.",
                            Purpose = "Order Notification",
                            Subject = "A holiday order has been generated!",
                            Template = "A holiday order for {employee.fullName} has been generated. Click this link to download it: {download.link}"
                        },
                        new
                        {
                            Id = 6,
                            Instructions = "{download.link} - A link to download request document.",
                            Purpose = "Request Notification",
                            Subject = "Your holiday request has been generated!",
                            Template = "You can download your holiday request document by clicking this link: {download.link}"
                        });
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthdayDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentAvailableLeaves")
                        .HasColumnType("int");

                    b.Property<int>("DaysOfVacation")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("FreeWorkDays")
                        .HasColumnType("float");

                    b.Property<DateTime>("HealthCheckDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)")
                        .HasMaxLength(15);

                    b.Property<int>("NextMonthAvailableLeaves")
                        .HasColumnType("int");

                    b.Property<double>("OvertimeHours")
                        .HasColumnType("float");

                    b.Property<int>("ParentalLeaveLimit")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("WorksFromDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthdayDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            CurrentAvailableLeaves = 0,
                            DaysOfVacation = 20,
                            Email = "gamma.holidays@gmail.com",
                            FreeWorkDays = 0.0,
                            HealthCheckDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Admin",
                            NextMonthAvailableLeaves = 0,
                            OvertimeHours = 0.0,
                            ParentalLeaveLimit = 0,
                            Position = "Administrator",
                            Status = 1,
                            Surname = "Admin",
                            WorksFromDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.FileRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FileRecords");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Holiday Policy.pdf",
                            Type = 1
                        });
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromInclusive")
                        .HasColumnType("datetime2");

                    b.Property<int>("OvertimeDays")
                        .HasColumnType("int");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RequestCreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToInclusive")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.InventoryCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Deprecation")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InventoryCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Deprecation = 6,
                            Name = "Furniture"
                        },
                        new
                        {
                            Id = 2,
                            Deprecation = 3,
                            Name = "Computers and telecommunication equipment"
                        },
                        new
                        {
                            Id = 3,
                            Deprecation = 4,
                            Name = "Other equipment"
                        },
                        new
                        {
                            Id = 4,
                            Deprecation = 0,
                            Name = "Software license"
                        });
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("InventoryCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("InventoryCategoryId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.InventoryItemTag", b =>
                {
                    b.Property<int>("InventoryItemId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("InventoryItemId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("InventoryItemsTags");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("XplicityApp.Infrastructure.Database.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Employee", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.Client", "Client")
                        .WithMany("Employees")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.Holiday", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.Employee", "Employee")
                        .WithMany("Holidays")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.InventoryItem", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.Employee", "Employee")
                        .WithMany("InventoryItems")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("XplicityApp.Infrastructure.Database.Models.InventoryCategory", "Category")
                        .WithMany("Items")
                        .HasForeignKey("InventoryCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.InventoryItemTag", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.InventoryItem", "InventoryItem")
                        .WithMany("InventoryItemsTags")
                        .HasForeignKey("InventoryItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("XplicityApp.Infrastructure.Database.Models.Tag", "Tag")
                        .WithMany("InventoryItemsTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("XplicityApp.Infrastructure.Database.Models.User", b =>
                {
                    b.HasOne("XplicityApp.Infrastructure.Database.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
