using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
namespace XplicityApp.Infrastructure.Repositories
{
    public interface IHolidayGuidsRepository : IRepository<HolidayGuid>
    {
        Task<string> GetGuid(int holidayId, int confirmerId, bool isAdmin);
        Task<HolidayGuid> GetHolidayGuid(string guid);
        Task<bool> DeleteGuids(int holidayId, bool isAdmin);
    }
}
