﻿using System;
using System.ComponentModel.DataAnnotations;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Holiday: BaseEntity
    {
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public HolidayType Type { get; set; }
        [Required]
        public DateTime FromInclusive { get; set; }
        [Required]
        public DateTime ToExclusive { get; set; }
        [Required]
        public int OvertimeDays { get; set; }
        [Required]
        public HolidayStatus Status { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required]
        public DateTime RequestCreatedDate { get; set; }
    }

}
