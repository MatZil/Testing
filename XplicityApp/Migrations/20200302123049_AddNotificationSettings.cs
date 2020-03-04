using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class AddNotificationSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BroadcastOwnBirthday = table.Column<bool>(nullable: false, defaultValue: true),
                    ReceiveBirthdayNotifications = table.Column<bool>(nullable: false, defaultValue: true),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSettings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationSettings",
                columns: new[] { "Id", "BroadcastOwnBirthday", "EmployeeId", "ReceiveBirthdayNotifications" },
                values: new object[] { 1, true, 1, true });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_EmployeeId",
                table: "NotificationSettings",
                column: "EmployeeId",
                unique: true);

             migrationBuilder.Sql(@"Declare @Ids table(Id int)
insert into @Ids Select Id from Employees

Declare @SettingsRecords table(RowID int IDENTITY(1, 1),EmployeeId int)

Declare @RecordsCount int = (Select Count(Employees.Id) from Employees)
Declare @RecordNumber int = 0
Declare @IsFound int = 0

While @RecordsCount > @RecordNumber
Begin
  Declare @SettingsRecordsCount int = (Select Count(NotificationSettings.EmployeeId) from NotificationSettings)
  insert into @SettingsRecords Select NotificationSettings.EmployeeId from NotificationSettings
  Declare @SettingsRecordNumber int = 0;

  while @SettingsRecordsCount > @SettingsRecordNumber
  begin
      if (Select EmployeeId from @SettingsRecords where RowID = @SettingsRecordNumber) = (select top 1 Id from @Ids)
          begin
              set @IsFound = 1
          end
      Set @SettingsRecordNumber = @SettingsRecordNumber + 1;
  end

  if @IsFound = 0
          begin
              Insert into NotificationSettings ([EmployeeId])
               Values((select top 1 Id from @Ids))
          end

DELETE top(1) from @Ids
Set @IsFound = 0;
SET @RecordNumber = @RecordNumber + 1
End"
);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSettings");
        }
    }
}
