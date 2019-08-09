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
    }
}
