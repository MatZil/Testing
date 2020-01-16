using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class EmailTemplatesSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "Instructions", "Purpose", "Subject", "Template" },
                values: new object[,]
                {
                    { 1, @"{admin.name} - Receiving admin's first name.
                {employee.fullName} - Employee's full name.
                {holiday.paid} - Whether or not holiday is paid.
                {holiday.type} - Holiday's type.
                {holiday.from} - Holiday's starting date.
                {holiday.to} - Holiday's ending date.
                {client.status} - Message that client has confirmed the holiday.
                {holiday.overtimeHours} - Message that employee wants to use some of his overtime hours for this holiday.
                {holiday.confirm} - Holiday's confirmation link.
                {holiday.decline} - Holiday's rejection link.", "Admin Confirmation", "An employee is requesting confirmation for holidays", @"Hello, {admin.name},

                An employee {employee.fullName} is intending to go on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {client.status} {holiday.overtimeHours}

                Click this link to confirm: {holiday.confirm}
                Click this link to decline: {holiday.decline}" },
                    { 2, @"{client.name} - Receiving client's first name.
                 {employee.fullName} - Employee's full name.
                {holiday.type} - Holiday's type.
                {holiday.from} - Holiday's starting date.
                {holiday.to} - Holiday's ending date.
                {holiday.confirm} - Holiday's confirmation link.
                {holiday.decline} - Holiday's rejection link.", "Client Confirmation", "One of your employees is requesting confirmation for holidays", @"Hello, {client.name},

                An employee {employee.fullName} is intending to go on {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive).

                Click this link to confirm: {holiday.confirm}
                Click this link to decline: {holiday.decline}" },
                    { 3, @"{client.name} - Individual team's client's name.
                {employee.fullName} - Individual employee's full name.
                {holiday.paid} - Whether or not the holiday is paid.
                {holiday.type} - Holiday's type.
                {holiday.from} - Holiday's starting date.
                {holiday.to} - Holiday's beginning date.
                {holiday.overtimeHours} - Message about individual employee's overtime hours.

                Please use the first line for team's title, second line for individual employee's info.", "Monthly Holidays' Report", "Monthly Holidays' Report Grouped By Teams", @"{client.name} team's employees:

                {employee.fullName} went on {holiday.paid} {holiday.type} holidays from {holiday.from} to {holiday.to} (inclusive). {holiday.overtimeHours}." },
                    { 5, "{employee.fullName} - Employee's full name.", "Birthday Reminder", "One of your colleagues is having their birthday today!", "Your colleague {employee.fullName} is having their birthday today! Don't forget to congratulate them." },
                    { 4, @"{employee.fullName} - Employee's full name.
                {holiday.from} - Holiday's starting date.
                {holiday.to} - Holiday's ending date.", "Upcoming Holiday Reminder", "One of your colleagues is going away for holidays next workday!", "Your colleague {employee.fullName} is going away for holidays next workday from {holiday.from} to {holiday.to} (inclusive)." },
                    { 7, @"{employee.fullName} - Employee's full name.
                {download.link} - A link to download order document.", "Order Notification", "A holiday order has been generated!", "A holiday order for {employee.fullName} has been generated. Click this link to download it: {download.link}" },
                    { 6, "{download.link} - A link to download request document.", "Request Notification", "Your holiday request has been generated!", "You can download your holiday request document by clicking this link: {download.link}" }
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 12, 13, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2019, 12, 13, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
