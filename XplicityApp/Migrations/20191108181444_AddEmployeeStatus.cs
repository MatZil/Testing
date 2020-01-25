using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class AddEmployeeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Position", "Status", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local), "inga@xplicity.com", "Administrator", 1, new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Position", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 3, 0, 0, 0, 0, DateTimeKind.Local), "Inga@xplicity.com", "Position", new DateTime(2019, 11, 3, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
