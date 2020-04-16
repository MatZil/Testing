using System;

namespace XplicityApp.Dtos.Employees
{
    public class GetEmployeeBirthdayDto
    {
        public string FullName { get; set; }
        public int BirthdayYear { get; set; }
        public DateTime BirthdayDate { get; set; }
        public bool IsPublic { get; set; }
    }
}
