using System;
using Xplicity_Holidays.Infrastructure.Static_Files;

namespace Xplicity_Holidays.Dtos.Employees
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? ClientId { get; set; }
        public DateTime WorksFromDate { get; set; }
        public DateTime BirthdayDate { get; set; }
        public int DaysOfVacation { get; set; }
        public double FreeWorkDays { get; set; }
        public int ParentalLeaveLimit { get; set; }
        public int CurrentAvailableLeaves { get; set; } 
        public int NextMonthAvailableLeaves { get; set; } 
        public string Email { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }

        public DateTime HealthCheckDate { get; set; }

        public EmployeeStatus.Status Status { get; set; }
    }
}
