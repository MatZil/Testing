using System;
using System.ComponentModel.DataAnnotations;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class BackgroundTask : BaseEntity
    {
        [Required]
        public DateTime ExecutionDate { get; set; }
    }
}
