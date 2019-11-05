using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class HolidaysPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Holidays",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Holidays");
        }
    }
}
