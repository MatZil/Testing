using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByEmail(string email);

        List<Holiday> GetHolidays(int employeeId); 
    }
}
