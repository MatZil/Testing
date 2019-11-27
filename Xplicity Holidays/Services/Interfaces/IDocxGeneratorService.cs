using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IDocxGeneratorService
    {
        Task<string> GenerateHolidayDocx(int holidayId, HolidayDocumentType holidayDocumentType);
    }
}
