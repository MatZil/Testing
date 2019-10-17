using System.Collections.Generic;
using System.Threading.Tasks;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.GeneratePDFByTemplate;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IEmployeesService
    {
        Employee Authenticate(string email, string password);
        Task<GetEmployeeDto> GetById(int id);
        Task<ICollection<GetEmployeeDto>> GetAll();
        Task<NewEmployeeDto> Create(NewEmployeeDto newClient);
        Task Update(int id, UpdateEmployeeDto updateData);
        Task<bool> Delete(int id);
        Task GenerateByTemplate(int id, HolidayType holidayType, HolidayDocumentType holidayDocumentType);
    }
}
