using System;

namespace Xplicity_Holidays.Dtos
{
    public class NewEmployeeDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? TeamId { get; set; }
        public DateTime WorksFromDate { get; set; }
        public double DaysOfVacation { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
