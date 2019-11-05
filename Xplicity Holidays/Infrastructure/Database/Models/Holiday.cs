using System;
using System.ComponentModel.DataAnnotations;

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
        public string Status { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required]
        public DateTime RequestCreatedDate { get; set; }
    }

    public enum HolidayType
    {
        Annual,
        Parental,
        Science
    }
}
