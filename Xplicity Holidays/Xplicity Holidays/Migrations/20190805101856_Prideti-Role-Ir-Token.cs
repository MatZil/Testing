using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class PridetiRoleIrToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Employees");
        }
    }
}
