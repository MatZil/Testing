using System.Collections.Generic;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        //Returns the number of holiday days available for an employee
        double GetNumberOfHolidaysLeft(int id);

        List<(string, double)> GetAllEmployeesHolidaysLeft();
    }
}
