using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class removePaidAndUpdateEmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Holidays");

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{admin.name} - Receiving admin's first name.
{employee.fullName} - Employee's full name.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{client.status} - Message that client has confirmed the holiday.
{holiday.overtimeHours} - Message that employee wants to use some of his overtime hours for this holiday.
{holiday.confirm} - Holiday's confirmation or rejection link.", @"Hello, {admin.name},

An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

Click this link to confirm or decline: {holiday.confirm}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Template",
                value: @"{client.name} team's employees:

{employee.fullName} went on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Holidays",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{admin.name} - Receiving admin's first name.
{employee.fullName} - Employee's full name.
{holiday.paid} - Whether or not holiday is paid.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{client.status} - Message that client has confirmed the holiday.
{holiday.overtimeHours} - Message that employee wants to use some of his overtime hours for this holiday.
{holiday.confirm} - Holiday's confirmation or rejection link.", @"Hello, {admin.name},

An employee {employee.fullName} is intending to go on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

Click this link to confirm or decline: {holiday.confirm}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                column: "Template",
                value: @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}");
        }
    }
}
