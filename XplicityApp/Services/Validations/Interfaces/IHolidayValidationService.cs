using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;

namespace XplicityApp.Services.Validations.Interfaces
{
    public interface IHolidayValidationService
    {
        Task ValidateHolidayConfirmationReadiness(int holidayId, int confirmerId);
        Task ValidateNewHolidayConfirmationReadiness(NewHolidayDto holidayDto);
    }
}
