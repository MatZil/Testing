﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Users;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Infrastructure.Database;

namespace Xplicity_Holidays.Services
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
            return currentUser.Employee;
        }
    }
}
