﻿using System;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Holidays
{
    public class NewHolidayDto
    {
        public int EmployeeId { get; set; }
        public HolidayType Type { get; set; }
        public DateTime FromInclusive { get; set; }
        public DateTime ToInclusive { get; set; }
        public int OvertimeDays { get; set; }
    }
}
