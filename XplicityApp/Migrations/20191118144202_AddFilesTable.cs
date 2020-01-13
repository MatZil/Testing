using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class AddFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Holidays",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Name", "Surname", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 18, 0, 0, 0, 0, DateTimeKind.Local), "gamma.holidays@gmail.com", "Admin", "Admin", new DateTime(2019, 11, 18, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Holidays",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthdayDate", "Email", "Name", "Surname", "WorksFromDate" },
                values: new object[] { new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local), "inga@xplicity.com", "Inga", "Rana", new DateTime(2019, 11, 8, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
