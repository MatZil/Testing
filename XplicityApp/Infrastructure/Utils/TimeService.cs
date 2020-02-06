using Nager.Date;
using System;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Infrastructure.Utils
{
    public class TimeService : ITimeService
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

        public int GetWorkDays(DateTime fromInclusive, DateTime toInclusive)
        {
            var workDays = 0;

            while (fromInclusive <= toInclusive)
            {
                if (IsFreeWorkDay(fromInclusive))
                {
                    fromInclusive = fromInclusive.AddDays(1);
                    continue;
                }

                workDays++;
                fromInclusive = fromInclusive.AddDays(1);
            }

            return workDays;
        }

        public int GetCurrentYearWorkDays()
        {
            var currentYear = GetCurrentTime().Year;
            return GetWorkDays(new DateTime(currentYear, 1, 1), new DateTime(currentYear, 12, 31));
        }

        public int GetRemainingMonthWorkDays(DateTime fromInclusive)
        {
            var followingMonth = fromInclusive.AddMonths(1);
            var lastMonthDay = new DateTime(followingMonth.Year, followingMonth.Month, 1).AddDays(-1);
            return GetWorkDays(fromInclusive, lastMonthDay);
        }
    }
}
