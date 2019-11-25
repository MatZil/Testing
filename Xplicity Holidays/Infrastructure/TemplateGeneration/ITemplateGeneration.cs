using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.TemplateGeneration
{
    public interface ITemplateGeneration
    {
        Task<string> GenerateFileByTemplate(Holiday holiday, HolidayDocumentType holidayDocumentType);
    }
}
