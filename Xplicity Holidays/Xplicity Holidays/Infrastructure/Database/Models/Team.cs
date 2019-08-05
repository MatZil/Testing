using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Team: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public Client Client { get; set; }
    }
}
