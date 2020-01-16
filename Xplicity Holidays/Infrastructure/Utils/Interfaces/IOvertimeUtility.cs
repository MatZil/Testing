using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Utils.Interfaces
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