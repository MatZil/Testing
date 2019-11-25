using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface ITemplateGenerationService
    {
        Task<bool> GenerateHolidayDocx(int holidayId, HolidayDocumentType holidayDocumentType);
    }
}
