﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public EmployeesService(IEmployeeRepository repository, IMapper mapper, UserManager<User> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetEmployeeDto> GetById(int id)
        {
            var employee = await _repository.GetById(id);
            var employeeDto = _mapper.Map<GetEmployeeDto>(employee);

            return employeeDto;
        }

        public async Task<ICollection<GetEmployeeDto>> GetAll()
        {
            var employees = await _repository.GetAll();
            var employeesDto = _mapper.Map<GetEmployeeDto[]>(employees);

            return employeesDto;
        }

        public async Task<NewEmployeeDto> Create(NewEmployeeDto newEmployeeDto)
        {
            if (newEmployeeDto == null)
                throw new ArgumentNullException(nameof(newEmployeeDto));

            var password = newEmployeeDto.Password;

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_repository.FindByEmail(newEmployeeDto.Email).Result != null)
                throw new Exception("Email \"" + newEmployeeDto.Email + "\" is already taken");

            var newEmployee = _mapper.Map<Employee>(newEmployeeDto);


            await _repository.Create(newEmployee);

            var employeeDto = _mapper.Map<NewEmployeeDto>(newEmployee);

            User newUser = new User();
            newUser.Employee = newEmployee;
            newUser.Email = newEmployeeDto.Email;
            newUser.UserName = newEmployeeDto.Email;
            var result = await _userManager.CreateAsync(newUser, newEmployeeDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, newEmployeeDto.Role);
            }
            return employeeDto;
        }

        public async Task<bool> Delete(int id)
        {
            var item = await _repository.GetById(id);

            if (item == null)
                return false;

            var deleted = await _repository.Delete(item);

            return deleted;
        }

        public async Task Update(int id, UpdateEmployeeDto updateData)
        {
            if (updateData == null)
                throw new ArgumentNullException(nameof(updateData));

            var employeeToUpdate = await _repository.GetById(id);
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
            
            var currentRole = await _userManager.GetRolesAsync(userToUpdate); // Roles that the user has currently
            if (!await _userManager.IsInRoleAsync(userToUpdate, updateData.Role))
            {
                await _userManager.AddToRoleAsync(userToUpdate, updateData.Role);
                await _userManager.RemoveFromRolesAsync(userToUpdate, currentRole);
            }
            if (employeeToUpdate == null || userToUpdate == null)
                throw new InvalidOperationException();

            if (updateData.Email != employeeToUpdate.Email)
            {
                // email has changed so check if the new email is already taken
                if (_repository.FindByEmail(updateData.Email).Result != null)
                    throw new Exception("Email " + updateData.Email + " is already taken");
            }

            _mapper.Map(updateData, employeeToUpdate);
            await _repository.Update(employeeToUpdate);
        }

    }
}
