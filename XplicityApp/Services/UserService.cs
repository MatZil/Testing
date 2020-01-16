using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Users;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Services.Interfaces;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOvertimeUtility _overtimeUtility;

        public UserService(IOvertimeUtility overtimeUtility, UserManager<User> userManager)
        {
            _userManager = userManager;
            _overtimeUtility = overtimeUtility;
        }
        public async Task<User> Create(Employee newEmployee, NewEmployeeDto newEmployeeDto)
        {
            User newUser = new User
            {
                Employee = newEmployee,
                Email = newEmployee.Email,
                UserName = newEmployee.Email
            };

            var result = await _userManager.CreateAsync(newUser, newEmployeeDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, newEmployeeDto.Role);
                return newUser;
            }
            
            return null;
        }

        public async Task Update(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (userToUpdate == null)
            {
                throw new InvalidOperationException();
            }
            var currentRole = await _userManager.GetRolesAsync(userToUpdate); // Roles that the user has currently
            if (!await _userManager.IsInRoleAsync(userToUpdate, updateEmployeeDto.Role))
            {
                await _userManager.AddToRoleAsync(userToUpdate, updateEmployeeDto.Role);
                await _userManager.RemoveFromRolesAsync(userToUpdate, currentRole);
            }
        }

        public async Task ChangePassword(int id, UpdatePasswordDto updatePasswordDto)
        {
            
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (userToUpdate == null)
            {
                throw new InvalidOperationException();
            }
            var result = await _userManager.ChangePasswordAsync(userToUpdate, updatePasswordDto.CurrentPassword, updatePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task<Employee> GetCurrentUser(string email)
        {
            var currentUser = await _userManager.Users.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Email == email);
            if (currentUser == null)
            {
                throw new InvalidOperationException();
            }

            var currentEmployee = _overtimeUtility.AddOvertimeDetailsToEmployee(currentUser.Employee);
            return currentEmployee;
        }
    }
}
