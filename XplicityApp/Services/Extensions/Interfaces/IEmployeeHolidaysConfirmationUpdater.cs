using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Extensions.Interfaces
{
    public interface IEmployeeHolidaysConfirmationUpdater
    {
        Task UpdateEmployeesOvertime(GetHolidayDto holidayDto);
        Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto);
        Task UpdateParentalLeaves(GetHolidayDto holidayDto);
    }
}
