using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IFileService
    {
        Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType);
        Task<string> Upload(IFormFile fomFile, FileTypeEnum fileType);
        Task<string> GetByType(FileTypeEnum fileType);
    }
}
