﻿using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class EmailTemplate: BaseEntity
    {
        [Required]
        public string Purpose { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Template { get; set; }
    }
}
