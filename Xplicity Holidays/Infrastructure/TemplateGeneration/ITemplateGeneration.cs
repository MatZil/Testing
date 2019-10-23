using System.Threading.Tasks;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.TemplateGeneration
{
    public interface ITemplateGeneration
    {
        Task<string> GenerateFileByTemplate(int employeeId, HolidayType holidayType, HolidayDocumentType holidayDocumentType);
    }
}
