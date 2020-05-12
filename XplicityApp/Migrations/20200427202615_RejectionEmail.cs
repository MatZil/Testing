using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class RejectionEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "Instructions", "Purpose", "Subject", "Template" },
                values: new object[] { 8, @"{rejecter.status} - Status of the rejecter (administrator or client).
{rejecter.fullName} - Full name of the rejecter.
{rejection.reason} - The provided rejection reason.", "Rejection Notification", "Your holiday request has been rejected", "Your holiday request has been rejected by your {rejecter.status} {rejecter.fullName}. {rejection.reason}" });

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "973726ae-47f7-4190-b46b-92a6fa33e889-2a4d646c-7581-413a-a55e-8cc7387db3c0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "29fe0e74-eb8c-4b3c-a7ac-66a031fc2653-321a6858-a955-4961-96b2-ada0b1fcce9b");
        }
    }
}
