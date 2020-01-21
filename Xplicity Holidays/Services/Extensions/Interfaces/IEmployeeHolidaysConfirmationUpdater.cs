using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;

namespace Xplicity_Holidays.Services.Extensions.Interfaces
{
    public interface IEmployeeHolidaysConfirmationUpdater
    {
        Task UpdateEmployeesOvertime(GetHolidayDto holidayDto);
        Task UpdateEmployeesWorkdays(GetHolidayDto holidayDto);
        Task UpdateParentalLeaves(GetHolidayDto holidayDto);
    }
}
