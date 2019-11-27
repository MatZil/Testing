using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class File : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public FileTypeEnum Type { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

