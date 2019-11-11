﻿using System;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Dtos.Holidays
{
    public class GetHolidayDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public HolidayType Type { get; set; }
        public DateTime FromInclusive { get; set; }
        public DateTime ToExclusive { get; set; }
        public HolidayStatus Status { get; set; }
        public bool Paid { get; set; }
        public DateTime RequestCreatedDate { get; set; }
    }
}
