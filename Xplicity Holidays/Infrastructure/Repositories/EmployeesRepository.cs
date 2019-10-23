using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

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
            var employees = await Context.Employees.ToArrayAsync();

            return employees;
        }

        public async Task<Employee> GetById(int id)
        {
            var employee = await Context.Employees.FindAsync(id);

            return employee;
        }

        public async Task<int> Create(Employee newEmployee)
        {
            Context.Employees.Add(newEmployee);
            await Context.SaveChangesAsync();

            return newEmployee.Id;
        }

        public async Task<bool> Update(Employee employee)
        {
            Context.Employees.Attach(employee);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Employee employee)
        {
            Context.Employees.Remove(employee);
            var changes = await Context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<Employee> FindByEmail(string email)
        {
            var employee = await Context.Employees.SingleOrDefaultAsync(emp => emp.Email == email);

            return employee;
        }

        public List<Holiday> GetConfirmedHolidays(int employeeId)
        {
            var holidays = Context.Holidays.Where(holiday => 
                                                    holiday.EmployeeId == employeeId && holiday.Status == HolidayStatus.Confirmed).ToList();

            return holidays;
        }

        public async Task<Employee> FindAnyAdmin()
        {
            var admin = await Context.Employees.FirstOrDefaultAsync(employee => employee.Role == "admin");

            return admin;
        }
    }
}
