using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class Depreciation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "InventoryItems",
                newName: "OriginalPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "InventoryItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "OriginalPrice",
                table: "InventoryItems",
                newName: "Price");
        }
    }
}
