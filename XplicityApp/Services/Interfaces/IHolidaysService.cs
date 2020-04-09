using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Services.Interfaces
{
    public interface IHolidaysService
    {
        Task<GetHolidayDto> GetById(int id);
        Task<ICollection<GetHolidayDto>> GetAll();
        Task<int> Create(NewHolidayDto newClient);
        Task<bool> Update(int id, UpdateHolidayDto updatedHoliday);
        Task<bool> Delete(int id);
        Task<ICollection<GetHolidayDto>> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus);
        Task<string> GetEmployeeFullName(int employeeId);
        Task<string> GetClientFullName(int clientId);
        Task<string> GetConfirmerFullName(GetHolidayDto holidayDto);
        Task<List<GetHolidayDto>> GetConfirmedByMonth(DateTime selectedDate, int currentUserId);
    }
}
