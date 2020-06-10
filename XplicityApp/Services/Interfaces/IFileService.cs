using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType);
        Task Upload(IFormFile fomFile, FileTypeEnum fileType);
        Task<string> GetNewestPolicyPath();
        Task<FileRecord> GetById(int fileId);
        Task<FileRecord> GetByGuid(string guid);
        string GetRelativeDirectory(FileTypeEnum fileType);
        string GetRelativeBlob(FileTypeEnum fileType);
        Task<string> GetDownloadLink(int fileId);
    }
}
