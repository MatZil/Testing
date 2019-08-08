using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using System.Linq;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class EmployeesRepository: IEmployeeRepository
    {
        protected readonly HolidayDbContext Context;

        public EmployeesRepository(HolidayDbContext context)
        {
            Context = context;
        }

        public async Task<ICollection<Employee>> GetAll()
        {
            var items = await Context.Employees.ToArrayAsync();

            return items;
        }

        public async Task<Employee> GetById(int id)
        {
            var items = await Context.Employees.FindAsync(id);

            return items;
        }

        public async Task<int> Create(Employee entity)
        {
            Context.Employees.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> Update(Employee entity)
        {
            Context.Employees.Attach(entity);
            var changes = await Context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Delete(Employee entity)
        {
            Context.Employees.Remove(entity);
            var changes = await Context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<Employee> FindByEmail(string email)
        {
            var employee = await Context.Employees.SingleOrDefaultAsync(x => x.Email == email);
            return employee;
        }

        public List<Holiday> GetHolidays(int employeeId)
        {
            var holidays = Context.Holidays.Where(holiday => holiday.EmployeeId == employeeId).ToList();
            return holidays;
        }
    }
}
