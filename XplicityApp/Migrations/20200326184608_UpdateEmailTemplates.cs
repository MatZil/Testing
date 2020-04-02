using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class UpdateEmailTemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 2,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{client.name} - Receiving client's first name.
 {employee.fullName} - Employee's full name.
{holiday.type} - Holiday's type.
{holiday.from} - Holiday's starting date.
{holiday.to} - Holiday's ending date.
{holiday.confirm} - Holiday's confirmation or rejection link.", @"Hello, {client.name},

An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive).

Click this link to confirm or decline: {holiday.confirm}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
