using Microsoft.EntityFrameworkCore.Migrations;

namespace XplicityApp.Migrations
{
    public partial class RunScriptForSettingsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
  Declare @SettingsRecordNumber int = 1;

  while @SettingsRecordsCount >= @SettingsRecordNumber
  begin
      if (Select EmployeeId from @SettingsRecords where RowID = @SettingsRecordNumber) = (select top 1 Id from @Ids)
          begin
              set @IsFound = 1
          end
      Set @SettingsRecordNumber = @SettingsRecordNumber + 1;
  end

  if @IsFound = 0
          begin
               Insert into NotificationSettings ([BroadcastOwnBirthday],[ReceiveBirthdayNotifications],[EmployeeId])
               Values(1,1,(select top 1 Id from @Ids))
          end

DELETE top(1) from @Ids
Set @IsFound = 0;
SET @RecordNumber = @RecordNumber + 1
End"
);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
