using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xplicity_Holidays.Infrastructure.Static_Files;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class FileService : IFileService
    {
        public string Upload(IFormFile file, string fileType)
        {
            string folderName = string.Empty;
            switch (fileType)
            {
                case "HOLIDAY_POLICY":
                    folderName = FilePath.HOLIDAY_POLICY;
                    DirectoryInfo directory = new DirectoryInfo(folderName);
                    foreach (FileInfo fileInfo in directory.GetFiles())
                    {
                        fileInfo.Delete();
                    }
                    break;
                case "WORD_DOCUMENT":
                    folderName = FilePath.WORD_DOCUMENT;
                    break;
                case "IMAGE":
                    folderName = FilePath.IMAGE;
                    break;
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var filePath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return filePath;
            }
            return string.Empty;
        }
    }
}
