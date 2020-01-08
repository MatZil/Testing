using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class MakeInventoryExpiryDateNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "InventoryItems",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 12, 12, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2019, 12, 12, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 12, 6, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2019, 12, 6, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
