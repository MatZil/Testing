using System;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Infrastructure.Utils
{
    public class OvertimeUtility : IOvertimeUtility
    {
        private int _hoursInWorkday;

        public OvertimeUtility(IConfiguration configuration)
        {
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
            return overtimeHours / _hoursInWorkday;
        }

        public double ConvertOvertimeDaysToHours(double overtimeDays)
        {
            return overtimeDays * _hoursInWorkday;
        }

        public Employee AddOvertimeDaysToEmployee(Employee employee)
        {
            employee.OvertimeDays = Math.Floor(ConvertOvertimeHoursToDays(employee.OvertimeHours));

            return employee;
        }

        public GetHolidayDto AddOvertimeDetailsToHoliday(GetHolidayDto holiday)
        {
            holiday.OvertimeHours = Math.Round(ConvertOvertimeDaysToHours(holiday.OvertimeDays), 2);

            return holiday;
        }
    }
}
