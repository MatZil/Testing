using System.Threading.Tasks;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Users;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(Employee newEmployee, NewEmployeeDto newEmployeeDto);

        Task Update(int id, UpdateEmployeeDto updateEmployeeDto);

        Task ChangePassword(int id, UpdatePasswordDto updatePasswordDto);

        Task ChangeEmail(int id, string updatedEmail);
        Task<Employee> GetCurrentUser(string email);
        Task<string> GetUserRole(int id);
    }
}
