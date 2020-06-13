using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class AddCreationDateToSurveys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Surveys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "2db351c5-104b-49be-968c-38368cf5aeaa-faa7caae-25d7-4b8a-9adf-164958533c8d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Surveys");

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "973726ae-47f7-4190-b46b-92a6fa33e889-2a4d646c-7581-413a-a55e-8cc7387db3c0");
        }
    }
}
