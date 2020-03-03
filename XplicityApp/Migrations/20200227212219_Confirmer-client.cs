using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class Confirmerclient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmerId",
                table: "Holidays");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmerAdminId",
                table: "Holidays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmerClientId",
                table: "Holidays",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmerAdminId",
                table: "Holidays");

            migrationBuilder.DropColumn(
                name: "ConfirmerClientId",
                table: "Holidays");

            migrationBuilder.AddColumn<int>(
                name: "ConfirmerId",
                table: "Holidays",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
