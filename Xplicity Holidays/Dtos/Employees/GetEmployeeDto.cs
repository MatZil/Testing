using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string Email { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
    }
}
