using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class EmployeeFreeWorkdays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FreeWorkDays",
                table: "Employees",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeWorkDays",
                table: "Employees");
        }
    }
}
