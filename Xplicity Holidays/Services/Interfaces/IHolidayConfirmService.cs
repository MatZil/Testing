using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Holidays;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IHolidayConfirmService
    {
        Task<bool> RequestClientApproval(NewHolidayDto holidayDto, int holidayId);
        Task<bool> RequestAdminApproval(int holidayId, string clientStatus);
        Task<bool> CreateRequestPdf(NewHolidayDto holidayDto, int holidayId);
        Task<bool> CreateOrderPdf(int holidayId);
    }
}
