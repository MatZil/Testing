using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class addArchivedToInventoryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "InventoryItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "InventoryItems");
        }
    }
}
