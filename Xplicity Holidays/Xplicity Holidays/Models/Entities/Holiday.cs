using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Models.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public int Days { get; set; }
    }
}
