using System;

namespace Xplicity_Holidays.Infrastructure.Utils
{
    public class TimeService: ITimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
