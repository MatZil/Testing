using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class Confirmer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmerId",
                table: "Holidays",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { @"{confirmer.fullName} - Full name of the administrator who confirmed the request.
{download.link} - A link to download request document.", "Your holiday request has been confirmed by {confirmer.fullName}. You can download your holiday request document by clicking this link: {download.link}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmerId",
                table: "Holidays");

            migrationBuilder.UpdateData(
                table: "EmailTemplates",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Instructions", "Template" },
                values: new object[] { "{download.link} - A link to download request document.", "You can download your holiday request document by clicking this link: {download.link}" });
        }
    }
}
