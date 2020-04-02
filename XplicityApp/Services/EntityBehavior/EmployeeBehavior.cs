using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.EntityBehavior
{
    public static class EmployeeBehavior
    {
        private const int DailyHourLimit = 8;

        public static double GetOvertimeDays(this Employee employee) => employee.OvertimeHours / DailyHourLimit;

        public static bool IsSamePerson(this Employee employee, Employee other) => employee.Email.Equals(other.Email);

    }
}