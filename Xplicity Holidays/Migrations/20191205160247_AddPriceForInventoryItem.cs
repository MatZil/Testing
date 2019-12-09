using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class AddPriceForInventoryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "InventoryItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "InventoryItems");

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
