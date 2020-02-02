using System;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface ITimeService
    {
        DateTime GetCurrentTime();
        int GetWorkDays(DateTime from, DateTime to);
        bool IsFreeWorkDay(DateTime freeWorkDay);
    }
}
