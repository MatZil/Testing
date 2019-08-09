using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        //Returns the number of holiday days available for an employee
        double GetNumberOfHolidaysLeft(int id);

        List<(string, double)> GetAllEmployeesHolidaysLeft();
    }
}
