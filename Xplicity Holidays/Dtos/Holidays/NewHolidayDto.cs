﻿using System;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Dtos.Holidays
{
    public class NewHolidayDto
    {
        public int EmployeeId { get; set; }
        public HolidayType Type { get; set; }
        public DateTime FromInclusive { get; set; }
        public DateTime ToExclusive { get; set; }
    }
}
