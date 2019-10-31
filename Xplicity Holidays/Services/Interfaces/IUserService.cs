using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Users;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(Employee newEmployee, NewEmployeeDto newEmployeeDto);

        Task Update(int id, UpdateEmployeeDto updateEmployeeDto);

        Task ChangePassword(int id, UpdatePasswordDto updatePasswordDto);
    }
}
