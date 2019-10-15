using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xplicity_Holidays.Infrastructure.Database.Models
{
    public class Employee: BaseEntity
    {
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Surname { get; set; }
        public Client Client { get; set; }
        public int? ClientId { get; set; }
        [Required]
        public DateTime WorksFromDate { get; set; }
        [Required]
        public DateTime BirthdayDate { get; set; }
        [Required]
        public int DaysOfVacation { get; set; } // An amount of free workdays over a year (either 20 or 25)
        [Required]
        public double FreeWorkDays { get; set; } //Current amount of free workdays left.
        [Required]
        public int ParentalLeaveLimit { get; set; } //Maximum amount of parental leaves employee can get in one month.
        [Required]
        public int CurrentAvailableLeaves { get; set; } //Number of parental leaves employee can get during current month.
        [Required]
        public int NextMonthAvailableLeaves { get; set; } //Number of parental leaves employee can get during next month.
        [Required]
        [MinLength(4)]
        public string Email { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        [Required]
        public string Position { get; set; }
        public ICollection<Holiday> Holidays { get; set; }

    }
}
