using System;
using System.ComponentModel.DataAnnotations;
using XplicityApp.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace XplicityApp.Infrastructure.Database.Models
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
        public DateTime ToInclusive { get; set; }
        [Required]
        public int OvertimeDays { get; set; }
        [Required]
        public HolidayStatus Status { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required]
        public DateTime RequestCreatedDate { get; set; }
        public int ConfirmerAdminId { get; set; }
        public int ConfirmerClientId { get; set; }

        [NotMapped]
        public double OvertimeHours { get; set; }

#nullable enable
        public string? RejectionReason { get; set; }
    }
}
