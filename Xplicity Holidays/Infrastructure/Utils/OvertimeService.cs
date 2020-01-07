using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using System;

namespace Xplicity_Holidays.Infrastructure.Utils
{
    public class OvertimeService
    {
        public string GetOvertimeSentence(string initialSentence, double overtimeHours)
        {
            var overtime = Math.Round(overtimeHours, 2).ToString();
            var overtimeSentence = string.Empty;
            if (overtimeHours > 0)
                overtimeSentence = initialSentence.Replace("{holiday.overtimeHours}", overtime);

            return overtimeSentence;
        }
    }
}
