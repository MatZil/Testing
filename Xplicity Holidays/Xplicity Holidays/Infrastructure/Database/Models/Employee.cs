using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Employee: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        [Required]
        public DateTime WorksFromDate { get; set; }
        [Required]
        public DateTime BirthdayDate { get; set; }
        [Required]
        public double DaysOfVacation { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public ICollection<Holiday> Holidays { get; set; }
    }
}
