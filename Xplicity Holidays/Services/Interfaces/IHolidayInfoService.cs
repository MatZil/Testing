using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        //Returns the number of holiday days available for an employee
        double GetNumberOfHolidaysLeft(int id);
        ICollection<HolidaysLeftDto> GetAllEmployeesHolidaysLeft();
    }
}
