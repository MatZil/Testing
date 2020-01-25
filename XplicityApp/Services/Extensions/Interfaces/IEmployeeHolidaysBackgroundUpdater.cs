using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Services.Extensions.Interfaces
{
    public interface IEmployeeHolidaysBackgroundUpdater
    {
        Task AddFreeWorkDays(ICollection<Employee> employees, ITimeService _timeService, IEmployeeRepository _repository);
        Task ResetParentalLeaves(ICollection<Employee> employees, ITimeService timeService, IEmployeeRepository repository);
    }
}
