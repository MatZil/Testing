using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Validations.Interfaces
{
    public interface IHolidayValidationService
    {
        Task ValidateHolidayConfirmationReadiness(int id);
        Task ValidateNewHolidayConfirmationReadiness(NewHolidayDto holidayDto);
    }
}
