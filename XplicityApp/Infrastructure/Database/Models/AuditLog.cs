using System;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(6000)]
        public string Data { get; set; }
        [Required]
        public string EntityType { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string User { get; set; }
    }
}
