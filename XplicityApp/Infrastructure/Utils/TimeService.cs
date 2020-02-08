using Nager.Date;
using System;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Infrastructure.Utils
{
    public class TimeService: ITimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public bool IsFreeWorkDay(DateTime workDay)
        {
            if (workDay.DayOfWeek == DayOfWeek.Saturday || workDay.DayOfWeek == DayOfWeek.Sunday
                                                || DateSystem.IsPublicHoliday(workDay, CountryCode.LT))
            {
                return true;
            }
            return false;
        }

        public int GetWorkDays(DateTime from, DateTime to)
        {
            var workDays = 0;

            while (from < to)
            {
                if (IsFreeWorkDay(from))
                {
                    from = from.AddDays(1);
                    continue;
                }

                workDays++;
                from = from.AddDays(1);
            }

            return workDays;
        }

        public DateTime GetNextWorkDay(DateTime currentTime)
        {
            var nextWorkDay = currentTime.AddDays(1);

            while (IsFreeWorkDay(nextWorkDay))
            {
                nextWorkDay = nextWorkDay.AddDays(1);
            }

            return nextWorkDay;
        }
    }
}
