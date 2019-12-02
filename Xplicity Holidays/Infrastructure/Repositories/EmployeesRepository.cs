using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Repositories
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
                                                    holiday.EmployeeId == employeeId && holiday.Status == HolidayStatus.Confirmed).ToList();

            return holidays;
        }

        public async Task<Employee> FindAnyAdmin()
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            return await Context.Employees.Where(employees => employees.Id == users[0].EmployeeId).SingleOrDefaultAsync();
        }

        public async Task<ICollection<InventoryItem>> GetEquipmentList(int employeeId)
        {
            var employee = await Context.Employees.Include(e => e.InventoryItems)
                .Where(e => e.Id == employeeId).SingleOrDefaultAsync();
            if (employee == null)
            {
                throw new ArgumentNullException();
            }

            return employee.InventoryItems;
        }
    }
}
