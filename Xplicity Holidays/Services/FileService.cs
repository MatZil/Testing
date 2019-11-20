using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Static_Files;
using Xplicity_Holidays.Services.Interfaces;
using File = Xplicity_Holidays.Infrastructure.Database.Models.File;

namespace Xplicity_Holidays.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public string Upload(IFormFile file, string fileType)
        {
            var fileToCreate = new File
            {
                Name = file.FileName,
                Type = fileType,
                IsValid = true,
                CreatedAt = DateTime.Now
            };
            var fileId = _fileRepository.Create(fileToCreate).Result;
            string folderName = string.Empty;
            switch (fileType)
            {
                case "HOLIDAY_POLICY":
                    folderName = Path.Combine(FilePath.HOLIDAY_POLICY, fileId.ToString());
                    break;
                case "WORD_DOCUMENT":
                    
                    folderName = Path.Combine(FilePath.WORD_DOCUMENT, fileId.ToString());
                    break;
                case "IMAGE":
                    folderName = Path.Combine(FilePath.IMAGE, fileId.ToString());
                    break;
            }
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }
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

        public async Task<string> GetByType(string fileType)
        {
            var file = await _fileRepository.FindByType(fileType);
            var folderName = string.Empty;
            switch (fileType)
            {
                case "HOLIDAY_POLICY":
                    folderName = Path.Combine(FilePath.HOLIDAY_POLICY, file.Id.ToString());
                    break;
                case "WORD_DOCUMENT":
                    folderName = Path.Combine(FilePath.WORD_DOCUMENT, file.Id.ToString());
                    break;
                case "IMAGE":
                    folderName = Path.Combine(FilePath.IMAGE, file.Id.ToString());
                    break;
            }

            var filePath = Path.Combine(folderName,file.Name).Replace(@"\","/");
            return filePath;
        }
    }
}
