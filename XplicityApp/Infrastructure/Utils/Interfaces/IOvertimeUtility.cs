using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface IOvertimeUtility
    {
        string GetOvertimeSentence(string initialSentence, int overtimeDays);
        double ConvertOvertimeHoursToDays(double overtimeHours);
        double ConvertOvertimeDaysToHours(double overtimeDays);
        Employee AddOvertimeDaysToEmployee(Employee employee);
        GetHolidayDto AddOvertimeDetailsToHoliday(GetHolidayDto holiday);
    }
}