using System;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(6000)]
        public string AuditData { get; set; }
        [Required]
        public string EntityType { get; set; }
        [Required]
        public DateTime AuditDate { get; set; }
        [Required]
        public string AuditUser { get; set; }
    }
}
