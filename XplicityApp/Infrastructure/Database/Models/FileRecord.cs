using System;
using System.ComponentModel.DataAnnotations;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class FileRecord : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public FileTypeEnum Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

