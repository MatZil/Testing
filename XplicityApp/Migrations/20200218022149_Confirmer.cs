using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class Confirmer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmerFullName",
                table: "Holidays",
                nullable: true);

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
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.", @"Hello, {admin.name},

An employee {employee.fullName} is intending to go on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{client.name} - Receiving client's first name.
 {employee.fullName} - Employee's full name.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.", @"Hello, {client.name},

An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive).

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{client.name} - Individual team's client's name.
{employee.fullName} - Individual employee's full name.
{holiday.paid} - Whether or not the holiday is paid.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's beginning date.
{holiday.overtimeHours} - Message about individual employee's overtime hours.

Please use the first line for team's title, second line for individual employee's info.", @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}." });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Instructions",
                value: @"{employee.fullName} - Employee's full name.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.");

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "Instructions",
                value: @"{employee.fullName} - Employee's full name.
{download.link} - A link to download order document.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmerFullName",
                table: "Holidays");

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
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.", @"Hello, {admin.name},

An employee {employee.fullName} is intending to go on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{client.name} - Receiving client's first name.
 {employee.fullName} - Employee's full name.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{holiday.confirm} - Holiday's confirmation link.
{holiday.decline} - Holiday's rejection link.", @"Hello, {client.name},

An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive).

Click this link to confirm: {holiday.confirm}
Click this link to decline: {holiday.decline}" });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{client.name} - Individual team's client's name.
{employee.fullName} - Individual employee's full name.
{holiday.paid} - Whether or not the holiday is paid.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's beginning date.
{holiday.overtimeHours} - Message about individual employee's overtime hours.

Please use the first line for team's title, second line for individual employee's info.", @"{client.name} team's employees:

{employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}." });

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 4,
                column: "Instructions",
                value: @"{employee.fullName} - Employee's full name.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.");

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 7,
                column: "Instructions",
                value: @"{employee.fullName} - Employee's full name.
{download.link} - A link to download order document.");
        }
    }
}
