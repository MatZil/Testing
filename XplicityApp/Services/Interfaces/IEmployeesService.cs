﻿using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IEmployeesService
    {
        Task<GetEmployeeDto> GetById(int id);
        Task<ICollection<GetEmployeeDto>> GetAll();
        Task<NewEmployeeDto> Create(NewEmployeeDto newClient);
        Task Update(int id, UpdateEmployeeDto updateData);
        Task<bool> Delete(int id);
        Employee AddOvertimeDays(Employee employee);
        bool HasActiveUnpaidHoliday(int id);
        Task<bool> EmailExists(string email);
    }
}
