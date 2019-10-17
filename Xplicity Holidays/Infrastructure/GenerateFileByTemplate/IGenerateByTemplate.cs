using System.Threading.Tasks;
using Xplicity_Holidays.Constants;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.GeneratePDF
{
    public interface IGenerateByTemplate
    {
        Task<string> GenerateFileByTemplate(int employeeId, HolidayType holidayType, HolidayDocumentType holidayDocumentType);
    }
}
