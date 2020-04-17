using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

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
        Task<List<GetEmployeeBirthdayDto>> GetBirthdaysByMonth(DateTime selectedDate, int currentUserId);
        Task<List<GetEmployeeBirthdayDto>> GetByRole(int currentUserId, List<GetEmployeeBirthdayDto> birthdays);
        Task<ICollection<GetEmployeeDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus);
    }
}
