using System;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Dtos.Employees
{
    public class NewEmployeeDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? ClientId { get; set; }
        public DateTime WorksFromDate { get; set; }
        public DateTime BirthdayDate { get; set; }
        public int DaysOfVacation { get; set; }
        public int ParentalLeaveLimit { get; set; }
        public int OvertimeHours { get; set; }
        public double FreeWorkDays { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
        public bool IsManualHolidaysInput { get; set; }

        public DateTime HealthCheckDate { get; set; }

        public EmployeeStatusEnum Status { get; set; }
    }
}
