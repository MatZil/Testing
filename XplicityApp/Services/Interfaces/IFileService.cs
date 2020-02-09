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
        string GetRelativeDirectory(FileTypeEnum fileType);
        string GetDownloadLink(int fileId);
    }
}
