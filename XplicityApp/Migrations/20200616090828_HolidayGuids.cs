using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class HolidayGuids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HolidayGuids",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayId = table.Column<int>(nullable: false),
                    ConfirmerId = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    Guid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayGuids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayGuids_Holidays_HolidayId",
                        column: x => x.HolidayId,
                        principalTable: "Holidays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "3eb1d829-f892-4108-aefd-54aaa7789ad8");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayGuids_HolidayId",
                table: "HolidayGuids",
                column: "HolidayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayGuids");

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "ebfd646e-cb37-4966-81c2-9298ce7e33f1-d4f4889a-3a79-4f59-ab9b-064a8e7eb27e");
        }
    }
}
