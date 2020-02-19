using System;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Holidays
{
    public class UpdateHolidayDto
    {
        public int EmployeeId { get; set; }
        public HolidayType Type { get; set; }
        public DateTime FromInclusive { get; set; }
        public DateTime ToInclusive { get; set; }
        public int OvertimeDays { get; set; }
        public HolidayStatus Status { get; set; }
        public bool Paid { get; set; }
        public DateTime RequestCreatedDate { get; set; }
        public int ConfirmerId { get; set; }
        public string ConfirmerFullName { get; set; }
    }
}