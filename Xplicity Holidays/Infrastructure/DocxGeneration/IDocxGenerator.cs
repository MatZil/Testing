using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.DocxGeneration
{
    public interface IDocxGenerator
    {
        Task<FileRecord> GenerateDocx(Holiday holiday, Employee employee, FileTypeEnum holidayDocumentType);
    }
}
