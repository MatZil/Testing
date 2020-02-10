using System;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface ITimeService
    {
        DateTime GetCurrentTime();
        int GetWorkDays(DateTime from, DateTime to);
        int GetCurrentYearWorkDays();
        int GetRemainingMonthWorkDays(DateTime fromInclusive);
        bool IsFreeWorkDay(DateTime freeWorkDay);
        DateTime GetNextWorkDay(DateTime nextWorkDay);
    }
}
