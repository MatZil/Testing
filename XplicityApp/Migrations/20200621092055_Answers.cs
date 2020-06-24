using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class Answers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HolidayGuids");

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    AnswerText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "FileRecords",
                keyColumn: "Id",
                keyValue: 1,
                column: "Guid",
                value: "89882d8e-5395-445b-8163-cb5eeba3e7fe-43e00511-fb58-4448-8411-9098c1f6ed56");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.CreateTable(
                name: "HolidayGuids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmerId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolidayId = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
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
    }
}
