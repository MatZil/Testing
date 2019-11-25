using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(int holidayId);
        Task<bool> RequestAdminApproval(int holidayId, string clientStatus);
        Task<bool> CreateRequestDocx(NewHolidayDto holidayDto, int holidayId);
        Task<bool> CreateOrderDocx(int holidayId);
        Task ConfirmHoliday(int holidayId);
        Task<bool> IsValid(int id);
        Task<bool> IsValid(NewHolidayDto holidayDto);
    }
}
