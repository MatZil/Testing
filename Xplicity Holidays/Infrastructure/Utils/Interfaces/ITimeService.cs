using System;

namespace Xplicity_Holidays.Infrastructure.Utils.Interfaces
{
    public interface ITimeService
    {
        DateTime GetCurrentTime();
        int GetWorkDays(DateTime from, DateTime to);
    }
}
