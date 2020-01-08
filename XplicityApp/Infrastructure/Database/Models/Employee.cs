using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class Employee: BaseEntity
    {
        private double _oneOvertimeHour = 1.5;
        private int _dailyHourLimit = 8;

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
        public double OvertimeHours { get; set; } //Current amount of accumulated overtime hours.
        [NotMapped]
        public double OvertimeDays { get{return OvertimeHours * _oneOvertimeHour / _dailyHourLimit; } set {}} //Current amount of overtime days (converted from hours)
        [Required]
        public int ParentalLeaveLimit { get; set; } //Maximum amount of parental leaves employee can get in one month.
        [Required]
        public int CurrentAvailableLeaves { get; set; } //Number of parental leaves employee can get during current month.
        [Required]
        public int NextMonthAvailableLeaves { get; set; } //Number of parental leaves employee can get during next month.
        [Required]
        [MinLength(4)]
        public string Email { get; set; }
        [NotMapped]
        public string Token { get; set; }
        public string Position { get; set; }
        public ICollection<Holiday> Holidays { get; set; }

        public DateTime HealthCheckDate { get; set; }

        public EmployeeStatusEnum Status { get; set; }

        public ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
