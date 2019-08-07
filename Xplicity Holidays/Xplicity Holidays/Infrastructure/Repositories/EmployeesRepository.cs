using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class EmployeesRepository: RepositoryBase<Employee>
    {
        protected override DbSet<Employee> ItemSet { get; }

        public EmployeesRepository(HolidayDbContext context) : base(context)
        {
            ItemSet = context.Employees;
        }
    }
}
