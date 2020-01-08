using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Services.Interfaces
{
    public interface IDocxGeneratorService
    {
        Task<int> GenerateHolidayDocx(int holidayId, FileTypeEnum holidayDocumentType);
    }
}
