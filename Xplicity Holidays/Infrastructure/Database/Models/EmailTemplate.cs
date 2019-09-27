using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
