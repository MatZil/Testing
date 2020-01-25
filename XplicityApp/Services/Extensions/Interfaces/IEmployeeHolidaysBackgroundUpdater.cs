using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Services.Extensions.Interfaces
{
    public interface IEmployeeHolidaysBackgroundUpdater
    {
        Task AddFreeWorkDays(ITimeService timeService, IEmployeeRepository employeeRepository);
        Task ResetParentalLeaves(ITimeService timeService, IEmployeeRepository employeeRepository);
    }
}
