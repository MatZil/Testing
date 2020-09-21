using Microsoft.Extensions.Configuration;
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

        public bool IsWorkDay(DateTime workDay)
        {
            if (workDay.DayOfWeek == DayOfWeek.Saturday || workDay.DayOfWeek == DayOfWeek.Sunday
                                                || DateSystem.IsPublicHoliday(workDay, CountryCode.LT))
            {
                return false;
            }
            return true;
        }

        public int GetWorkDays(DateTime fromInclusive, DateTime toInclusive)
        {
            var workDays = 0;

            while (fromInclusive <= toInclusive)
            {
                if (!IsWorkDay(fromInclusive))
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

        public DateTime GetNextWorkDay(DateTime currentTime)
        {
            var nextWorkDay = currentTime.AddDays(1);

            while (!IsWorkDay(nextWorkDay))
            {
                nextWorkDay = nextWorkDay.AddDays(1);
            }

            return nextWorkDay;
        }

        public int ConvertYearsToDays(int years)
        {
            int daysInYear = 365;
            return years * daysInYear;
        }

        public DateTime GetCalendarDateFrom(IConfiguration configuration, DateTime selectedDate)
        {
            var numberOfLastMonthDays = configuration.GetValue<int>("CalendarConfig:NumberOfLastMonthDays");

            var yearFrom = selectedDate.AddMonths(-1).Year;
            var monthFrom = selectedDate.AddMonths(-1).Month;
            var daysInLastMonth = DateTime.DaysInMonth(yearFrom, monthFrom);
            var dayFrom = daysInLastMonth - numberOfLastMonthDays;
            var dateFrom = new DateTime(yearFrom, monthFrom, dayFrom);

            return dateFrom;
        }

        public DateTime GetCalendarDateTo(IConfiguration configuration, DateTime selectedDate)
        {
            var numberOfNextMonthDays = configuration.GetValue<int>("CalendarConfig:NumberOfNextMonthDays");

            var yearTo = selectedDate.AddMonths(1).Year;
            var monthTo = selectedDate.AddMonths(1).Month;
            var dateTo = new DateTime(yearTo, monthTo, numberOfNextMonthDays);

            return dateTo;
        }
    }
}
