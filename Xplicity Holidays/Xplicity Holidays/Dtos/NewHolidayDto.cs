using System;

namespace Xplicity_Holidays.Dtos
{
    public class HolidayAddDto
    {
        public int EmployeeId { get; set; }
        public string Type { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Days { get; set; }
    }
}
