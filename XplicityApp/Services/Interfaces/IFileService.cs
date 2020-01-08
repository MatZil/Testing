using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType);
        Task<string> Upload(IFormFile fomFile, FileTypeEnum fileType);
        Task<string> GetByType(FileTypeEnum fileType);
        Task<FileRecord> GetById(int fileId);
        string GetDirectory(FileTypeEnum fileType);
        string GetDownloadLink(int fileId);
    }
}
