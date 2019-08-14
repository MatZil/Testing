using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        //Returns the number of holiday days available for an employee
        double GetNumberOfHolidaysLeft(int id);
        ICollection<HolidaysLeftDto> GetAllEmployeesHolidaysLeft();
        Task<List<(Holiday, Client)>> GetClientsAndHolidays(ICollection<Holiday> holidays);
    }
}
