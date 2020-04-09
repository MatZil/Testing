using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Users;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Services.Interfaces;
using System.Linq;

namespace XplicityApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<User> Create(Employee newEmployee, NewEmployeeDto newEmployeeDto)
        {
            var newUser = new User
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
        public async Task ChangeEmail(int id, string updatedEmail)
        {
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (userToUpdate == null)
            {
                throw new InvalidOperationException();
            }
            await _userManager.SetEmailAsync(userToUpdate,updatedEmail);
        }

        public async Task<Employee> GetCurrentUser(string email)
        {
            var currentUser = await _userManager.Users.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Email == email);
            if (currentUser == null)
            {
                throw new InvalidOperationException();
            }
            return currentUser.Employee;
        }

        public async Task<string> GetUserRole(int id)
        {
            var user = await _userManager.Users.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

            var userRoles = await _userManager.GetRolesAsync(user);
            var firstUserRole = userRoles.FirstOrDefault();

            return firstUserRole;
        }
    }
}
