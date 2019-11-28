using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> Upload(IFormFile fomFile, FileTypeEnum fileType);

        Task<string> GetByType(FileTypeEnum fileType);
    }
}
