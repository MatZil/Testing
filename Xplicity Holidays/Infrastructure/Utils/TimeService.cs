using System;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Infrastructure.Utils
{
    public class TimeService: ITimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public int GetWorkDays(DateTime from, DateTime to)
        {
            var workDays = 0;

            while (from < to)
            {
                if (from.DayOfWeek == DayOfWeek.Saturday || from.DayOfWeek == DayOfWeek.Sunday)
                {
                    from = from.AddDays(1);
                    continue;
                }
                workDays++;
                from = from.AddDays(1);
            }

            return workDays;
        }
    }
}
