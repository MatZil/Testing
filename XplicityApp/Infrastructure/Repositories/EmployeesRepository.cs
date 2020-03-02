using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Repositories
{
    public class EmployeesRepository : IEmployeeRepository
    {
        protected readonly HolidayDbContext Context;
        private readonly UserManager<User> _userManager;
        public EmployeesRepository(HolidayDbContext context, UserManager<User> userManager)
        {
            Context = context;
            _userManager = userManager;
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
                                                    holiday.EmployeeId == employeeId && holiday.Status == HolidayStatus.AdminConfirmed).ToList();

            return holidays;
        }

        public async Task<ICollection<Employee>> GetAllAdmins()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return Context.Employees.AsEnumerable().Where(employee => users.FirstOrDefault(u => u.EmployeeId == employee.Id) != null).ToList();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await Context.Employees.Select(employee => employee.Email).ContainsAsync(email);
        }
    }
}
