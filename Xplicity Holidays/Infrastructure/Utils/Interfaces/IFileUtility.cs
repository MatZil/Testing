using System.Threading.Tasks;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Infrastructure.Utils.Interfaces
{
    public interface IFileUtility
    {
        Task<string> GetGeneratedDocxPath(int holidayId, FileTypeEnum holidayDocumentType);
        string ExtractNameFromPath(string pathToFile);
    }
}
