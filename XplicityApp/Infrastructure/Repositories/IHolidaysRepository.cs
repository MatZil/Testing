using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IHolidaysRepository : IRepository<Holiday>
    {
        Task<ICollection<Holiday>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus);
    }
}
