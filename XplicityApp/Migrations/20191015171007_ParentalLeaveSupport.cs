using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class ParentalLeaveSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentAvailableLeaves",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NextMonthAvailableLeaves",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentalLeaveLimit",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAvailableLeaves",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NextMonthAvailableLeaves",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ParentalLeaveLimit",
                table: "Employees");
        }
    }
}
