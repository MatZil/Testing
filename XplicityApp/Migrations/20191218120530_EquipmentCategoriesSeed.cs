using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class EquipmentCategoriesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Normative",
                table: "InventoryCategories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "InventoryItems",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "Deprecation",
                table: "InventoryCategories",
                nullable: true);

            migrationBuilder.InsertData(
                table: "InventoryCategories",
                columns: new[] { "Id", "Deprecation", "Name" },
                values: new object[,]
                {
                    { 1, 6, "Furniture" },
                    { 2, 3, "Computers and telecommunication equipment" },
                    { 3, 4, "Other equipment" },
                    { 4, 0, "Software license" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "InventoryCategories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Deprecation",
                table: "InventoryCategories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Normative",
                table: "InventoryCategories",
                nullable: false,
                defaultValue: 0);
        }
    }
}
