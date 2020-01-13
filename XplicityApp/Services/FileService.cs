using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;
        private readonly IWebHostEnvironment _environment;

        public FileService(IFileRepository fileRepository, IConfiguration configuration, ITimeService timeService, IWebHostEnvironment environment)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
            _timeService = timeService;
            _environment = environment;
        }

        public async Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType)
        {
            var fileRecordToCreate = new FileRecord
            {
                Name = fileName,
                Type = fileType,
                CreatedAt = _timeService.GetCurrentTime()
            };

            var fileId = await _fileRepository.Create(fileRecordToCreate);

            return fileId;
        }

        public async Task<string> Upload(IFormFile formFile, FileTypeEnum fileType)
        {
            if (formFile.Length > 0)
            {
                var fileId =  await CreateFileRecord(formFile.FileName, fileType);
                var file = await _fileRepository.GetById(fileId);

                var filePath = Path.Combine(GetDirectory(file.Type), file.Id.ToString());

                var pathToSave = Path.Combine(_environment.WebRootPath, filePath);
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

        public string GetDirectory(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.HolidayPolicy:
                    return _configuration["FileConfig:HolidayPolicyFolder"];

                case FileTypeEnum.Document:
                    return _configuration["FileConfig:DocumentsFolder"];

                case FileTypeEnum.Image:
                    return _configuration["FileConfig:ImagesFolder"];

                case FileTypeEnum.Request:
                    return _configuration["FileConfig:RequestsFolder"];

                case FileTypeEnum.Order:
                    return _configuration["FileConfig:OrdersFolder"];
            }

            return "";
        }
        public async Task<string> GetByType(FileTypeEnum fileType)
        {
            var file = await _fileRepository.FindByType(fileType);
            var folderName = Path.Combine(GetDirectory(file.Type), file.Id.ToString());
            var filePath = Path.Combine(folderName, file.Name).Replace(@"\", "/");
            return filePath;
        }

        public async Task<FileRecord> GetById(int fileId)
        {
            return await _fileRepository.GetById(fileId);
        }

        public string GetDownloadLink(int fileId)
        {
            return $"{_configuration["AppSettings:RootUrl"]}/api/files/{fileId}/download";
        }
    }
}
