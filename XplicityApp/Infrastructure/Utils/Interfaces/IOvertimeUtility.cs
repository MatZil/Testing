using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface IOvertimeUtility
    {
        string GetOvertimeSentence(string initialSentence, int overtimeDays);
        double ConvertOvertimeHoursToDays(double overtimeHours);
        double ConvertOvertimeDaysToHours(double overtimeDays);
        int GetHoursTillNextOvertimeDay(double overtimeDays);
        int GetMinutesTillNextOvertimeDay(double overtimeDays);
        Employee AddOvertimeDetailsToEmployee(Employee employee);
        //Holiday AddOvertimeDetailsToHoliday(Holiday holiday);
    }
}