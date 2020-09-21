using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class BackgroundTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackgroundTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundTasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BackgroundTasks",
                columns: new[] { "Id", "ExecutionDate" },
                values: new object[] { 1, new DateTime(2020, 9, 19, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
