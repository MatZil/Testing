using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface ITemplateGenerationService
    {
        Task GenerateHolidayPdf(int holidayId, HolidayDocumentType holidayDocumentType);
    }
}
