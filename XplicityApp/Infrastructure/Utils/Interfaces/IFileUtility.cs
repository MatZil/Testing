using System.Threading.Tasks;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface IFileUtility
    {
        Task<string> GetGeneratedDocxName(int holidayId, FileTypeEnum holidayDocumentType);
        string ExtractNameFromPath(string pathToFile);
    }
}
