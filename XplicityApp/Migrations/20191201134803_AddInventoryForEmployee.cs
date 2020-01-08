using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class AddInventoryForEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "InventoryItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_EmployeeId",
                table: "InventoryItems",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Employees_EmployeeId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItems_EmployeeId",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "InventoryItems");
        }
    }
}
