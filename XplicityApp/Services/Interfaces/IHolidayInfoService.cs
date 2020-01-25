using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidayInfoService
    {
        Task<List<(Holiday, Client)>> GetClientsAndHolidays(ICollection<Holiday> holidays);
    }
}
