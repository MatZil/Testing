using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Client: BaseEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string CompanyName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string OwnerName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string OwnerSurname { get; set; }
        [Required]
        [MinLength(4)]
        public string OwnerEmail { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(12)]
        public string OwnerPhone { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
