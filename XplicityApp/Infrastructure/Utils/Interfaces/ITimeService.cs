using System;
using Microsoft.Extensions.Configuration;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface ITimeService
    {
        DateTime GetCurrentTime();
        int GetWorkDays(DateTime from, DateTime to);
        int GetCurrentYearWorkDays();
        int GetRemainingMonthWorkDays(DateTime fromInclusive);
        bool IsWorkDay(DateTime freeWorkDay);
        DateTime GetNextWorkDay(DateTime nextWorkDay);
        int ConvertYearsToDays(int years);
        DateTime GetCalendarDateFrom(IConfiguration configuration, DateTime selectedDate);
        DateTime GetCalendarDateTo(IConfiguration configuration, DateTime selectedDate);
        DateTime AdjustBirthdayDateForCalendar(DateTime date);
    }
}
