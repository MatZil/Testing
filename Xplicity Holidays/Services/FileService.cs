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

        private async Task<int> CreateFileRecord(IFormFile file, string fileType)
        {
            var fileToCreate = new File
            {
                Name = file.FileName,
                Type = fileType,
                CreatedAt = DateTime.Now
            };
            var fileId = await _fileRepository.Create(fileToCreate);
            return fileId;
        }
        public string Upload(IFormFile formFile, string fileType)
        {
            if (formFile.Length > 0)
            {
                var fileId = CreateFileRecord(formFile, fileType).Result;
                var filePath = BuildFilePath(_fileRepository.GetById(fileId).Result);

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var fullPath = Path.Combine(pathToSave, formFile.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                return filePath;
            }

            return string.Empty;
        }

        private string BuildFilePath(File file)
        {
            string folderName = string.Empty;
            switch (file.Type)
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
            return folderName;
        }
        public async Task<string> GetByType(string fileType)
        {
            var file = await _fileRepository.FindByType(fileType);
            var folderName = BuildFilePath(file);
            var filePath = Path.Combine(folderName, file.Name).Replace(@"\", "/");
            return filePath;
        }
    }
}
