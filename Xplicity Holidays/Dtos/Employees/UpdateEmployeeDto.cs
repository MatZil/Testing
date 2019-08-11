using System;

namespace Xplicity_Holidays.Dtos.Employees
{
    public class UpdateEmployeeDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? ClientId { get; set; }
        public DateTime WorksFromDate { get; set; }
        public DateTime BirthdayDate { get; set; }
        public int DaysOfVacation { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
    }
}
