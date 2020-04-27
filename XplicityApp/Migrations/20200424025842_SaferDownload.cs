using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class SaferDownload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "FileRecords",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "29fe0e74-eb8c-4b3c-a7ac-66a031fc2653-321a6858-a955-4961-96b2-ada0b1fcce9b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "FileRecords");
        }
    }
}
