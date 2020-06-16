using System;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class HolidayGuid: BaseEntity
    {
        [Required]
        public int HolidayId { get; set; }
        [Required]
        public int? ConfirmerId { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        public string Guid { get; set; }
    }
}
