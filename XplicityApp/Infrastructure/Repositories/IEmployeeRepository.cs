using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByEmail(string email);

        List<Holiday> GetConfirmedHolidays(int employeeId); 

        Task<ICollection<Employee>> GetAllAdmins();

    }
}
