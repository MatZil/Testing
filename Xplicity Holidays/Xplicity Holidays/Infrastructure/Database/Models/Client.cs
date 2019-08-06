using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Client: BaseEntity
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string OwnerSurname { get; set; }
        [Required]
        public string OwnerEmail { get; set; }
        [Required]
        public string OwnerPhone { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
