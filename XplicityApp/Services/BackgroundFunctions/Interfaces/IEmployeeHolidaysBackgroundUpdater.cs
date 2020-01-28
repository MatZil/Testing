using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.BackgroundFunctions.Interfaces
{
    public interface IEmployeeHolidaysBackgroundUpdater
    {
        Task AddFreeWorkDays(ICollection<Employee> employees);
        Task ResetParentalLeaves(ICollection<Employee> employees);
    }
}
