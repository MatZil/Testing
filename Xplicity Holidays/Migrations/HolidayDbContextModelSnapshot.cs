﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Xplicity_Holidays.Infrastructure.Database;

namespace Xplicity_Holidays.Migrations
{
    [DbContext(typeof(HolidayDbContext))]
    partial class HolidayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("OwnerEmail")
                        .IsRequired();

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("OwnerPhone")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<string>("OwnerSurname")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("CompanyName")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.EmailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Purpose")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("Template")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Purpose")
                        .IsUnique();

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthdayDate");

                    b.Property<int?>("ClientId");

                    b.Property<int>("DaysOfVacation");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("Position")
                        .IsRequired();

                    b.Property<string>("Role");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Token");

                    b.Property<DateTime>("WorksFromDate");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeId");

                    b.Property<DateTime>("FromInclusive");

                    b.Property<DateTime>("RequestCreatedDate");

                    b.Property<string>("Status")
                        .IsRequired();

                    b.Property<DateTime>("ToExclusive");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.Employee", b =>
                {
                    b.HasOne("Xplicity_Holidays.Infrastructure.Database.Models.Client", "Client")
                        .WithMany("Employees")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("Xplicity_Holidays.Infrastructure.Database.Models.Holiday", b =>
                {
                    b.HasOne("Xplicity_Holidays.Infrastructure.Database.Models.Employee", "Employee")
                        .WithMany("Holidays")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
