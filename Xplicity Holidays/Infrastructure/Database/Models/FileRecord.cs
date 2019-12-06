using System;
using System.ComponentModel.DataAnnotations;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Database.Models
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

