using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Models.Entities
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Employee> Employees { get; set; }
        public Client Client { get; set; }
        public int? ClientId { get; set; }
    }
}
