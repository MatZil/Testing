using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IFileService
    {
        string Upload(IFormFile fomFile, string fileType);
    }
}
