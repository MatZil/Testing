using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IEmployeesService
    {
        Task<GetEmployeeDto> GetById(int id);
        Task<ICollection<GetEmployeeDto>> GetAll();
        Task<NewEmployeeDto> Create(NewEmployeeDto newClient);
        Task Update(int id, UpdateEmployeeDto updateData);
        Task<bool> Delete(int id);
    }
}
