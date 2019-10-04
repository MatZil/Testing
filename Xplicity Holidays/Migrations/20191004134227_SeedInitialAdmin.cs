using Microsoft.EntityFrameworkCore.Migrations;

namespace Xplicity_Holidays.Migrations
{
    public partial class SeedInitialAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ebf2ee18-d97a-49fe-9301-e6cd31957140", "9ebf9b00-6a40-4b3f-9839-4c170a2c9ec4", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "53ec4767-d79f-452e-ab70-4bbe27c44fc0", 0, "c9fa0221-dde6-436f-8df4-339e0559126a", "inga@xplicity.com", true, false, null, "inga@xplicity.com", "inga@xplicity.com", "AQAAAAEAACcQAAAAEPeUL679nGeKPD6u7UZfV1CT+4Ycuh5f7K6h3iMhVAO+rNtcM/YyYjixKqYiC8lXng==", null, false, "", false, "inga@xplicity.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "53ec4767-d79f-452e-ab70-4bbe27c44fc0", "ebf2ee18-d97a-49fe-9301-e6cd31957140" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "53ec4767-d79f-452e-ab70-4bbe27c44fc0", "ebf2ee18-d97a-49fe-9301-e6cd31957140" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ebf2ee18-d97a-49fe-9301-e6cd31957140");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "53ec4767-d79f-452e-ab70-4bbe27c44fc0");
        }
    }
}
