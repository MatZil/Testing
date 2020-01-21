using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;

namespace Xplicity_Holidays.Services.Extensions.Interfaces
{
    public interface IEmployeeHolidaysBackgroundUpdater
    {
        Task AddFreeWorkDays(ICollection<Employee> employees, ITimeService _timeService, IEmployeeRepository _repository);
        Task ResetParentalLeaves(ICollection<Employee> employees, ITimeService timeService, IEmployeeRepository repository);
    }
}
