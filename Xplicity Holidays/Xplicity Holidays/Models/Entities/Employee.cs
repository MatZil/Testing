using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public Team Team { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }  
        [Required]
        public DateTime WorksFromDate { get; set; }
        [Required]
        public double DaysOfVacation { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public List<Holiday> Holidays { get; set; }
    }
}
