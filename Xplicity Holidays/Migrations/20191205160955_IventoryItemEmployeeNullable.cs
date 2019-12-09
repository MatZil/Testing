using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class IventoryItemEmployeeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "InventoryItems",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "InventoryItems",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
