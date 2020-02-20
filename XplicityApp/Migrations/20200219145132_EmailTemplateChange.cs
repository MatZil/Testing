using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class EmailTemplateChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Template",
                value: @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Template",
                value: @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}.");
        }
    }
}
