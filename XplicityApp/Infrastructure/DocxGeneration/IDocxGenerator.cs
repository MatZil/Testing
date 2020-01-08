using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.DocxGeneration
{
    public interface IDocxGenerator
    {
        Task<int> GenerateDocx(Holiday holiday, Employee employee, FileTypeEnum holidayDocumentType);
    }
}
