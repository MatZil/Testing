using System;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Infrastructure.Utils
{
    public class OvertimeUtility : IOvertimeUtility
    {
        private double _oneOvertimeHour;
        private int _hoursInWorkday;

        public OvertimeUtility(IConfiguration configuration)
        {
            _oneOvertimeHour = configuration.GetValue<double>("OvertimeConfig:OneOvertimeHour");
            _hoursInWorkday = configuration.GetValue<int>("OvertimeConfig:HoursInWorkday");
        }

        public string GetOvertimeSentence(string initialSentence, int overtimeDays)
        {
            var overtimeHours = Math.Round(ConvertOvertimeDaysToHours(overtimeDays), 2);
            var overtimeSentence = string.Empty;
            if (overtimeHours > 0)
            {
                overtimeSentence = initialSentence.Replace("{holiday.overtimeHours}", overtimeHours.ToString());
            }

            return overtimeSentence;
        }

        public double ConvertOvertimeHoursToDays(double overtimeHours)
        {
            return overtimeHours * _oneOvertimeHour / _hoursInWorkday;
        }

        public double ConvertOvertimeDaysToHours(double overtimeDays)
        {
            return overtimeDays * _hoursInWorkday / _oneOvertimeHour;
        }

        private double GetTimeTillNextOvertimeDay(double overtimeDays)
        {
            return Math.Abs(overtimeDays - (int)overtimeDays - 1) * _hoursInWorkday / _oneOvertimeHour;
        }

        public int GetHoursTillNextOvertimeDay(double overtimeDays)
        {
            return (int)GetTimeTillNextOvertimeDay(overtimeDays);
        }

        public int GetMinutesTillNextOvertimeDay(double overtimeDays)
        {
            int minutesInHour = 60;

            var timeTillNextOvertime = GetTimeTillNextOvertimeDay(overtimeDays);
            var hoursTillNextOvertime = GetHoursTillNextOvertimeDay(overtimeDays);
            return (int)Math.Ceiling((timeTillNextOvertime - hoursTillNextOvertime) * minutesInHour);
        }

        public Employee AddOvertimeDetailsToEmployee(Employee employee)
        {
            employee.OvertimeDays = ConvertOvertimeHoursToDays(employee.OvertimeHours);
            employee.NextOvertimeHours = GetHoursTillNextOvertimeDay(employee.OvertimeDays);
            employee.NextOvertimeMinutes = GetMinutesTillNextOvertimeDay(employee.OvertimeDays);

            return employee;
        }

        //public Holiday AddOvertimeDetailsToHoliday(Holiday holiday)
        //{
        //    holiday.OvertimeHours = ConvertOvertimeDaysToHours(holiday.OvertimeDays);

        //    return holiday;
        //}
    }
}
