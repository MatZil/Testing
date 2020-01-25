using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class OvertimeHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OvertimeDays",
                table: "Holidays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "OvertimeHours",
                table: "Employees",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Name", "Surname", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 28, 0, 0, 0, 0, DateTimeKind.Local), "gamma.holidays@gmail.com", "Admin", "Admin", new DateTime(2019, 11, 28, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OvertimeDays",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "OvertimeHours",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Name", "Surname", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local), "inga@xplicity.com", "Inga", "Rana", new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
