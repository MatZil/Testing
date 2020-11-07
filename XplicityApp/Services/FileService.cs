using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
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
        private readonly IAzureStorageService _azureStorageService;

        public FileService(IFileRepository fileRepository, IConfiguration configuration, ITimeService timeService,
            IAzureStorageService azureStorageService)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
            _timeService = timeService;
            _azureStorageService = azureStorageService;
        }

        public async Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType)
        {
            var guid = Guid.NewGuid().ToString();

            var fileRecordToCreate = new FileRecord
            {
                Guid = guid,
                Name = fileName,
                Type = fileType,
                CreatedAt = _timeService.GetCurrentTime()
            };

            var fileId = await _fileRepository.Create(fileRecordToCreate);

            return fileId;
        }

        public async Task Upload(IFormFile formFile, FileTypeEnum fileType)
        {
            if (formFile.Length > 0)
            {
                await CreateFileRecord(formFile.FileName, fileType);
                
                var containerName = GetBlobContainerName(fileType);
                var fileStream = formFile.OpenReadStream();
                await _azureStorageService.UploadBlob(containerName,
                    fileType == FileTypeEnum.HolidayPolicy
                        ? _configuration["FileConfig:HolidayPolicyFileName"]
                        : formFile.FileName, formFile.ContentType, fileStream);
                fileStream.Close();
            }
        }

        public string GetRelativeDirectory(FileTypeEnum fileType)
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

            return _configuration["FileConfig:UnknownFolder"];
        }
        public string GetBlobContainerName(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.HolidayPolicy:
                    return _configuration["BlobConfig:HolidayPolicy"];

                case FileTypeEnum.Document:
                    return _configuration["BlobConfig:Documents"];

                case FileTypeEnum.Image:
                    return _configuration["BlobConfig:Images"];

                case FileTypeEnum.Request:
                    return _configuration["BlobConfig:Requests"];

                case FileTypeEnum.Order:
                    return _configuration["BlobConfig:Orders"];
            }

            return _configuration["BlobConfig:Unknown"];
        }
        public string GetHolidayPolicyPath()
        {
            return _azureStorageService.GetBlobUrl("policy", _configuration["FileConfig:HolidayPolicyFileName"]);
        }

        public async Task<FileRecord> GetById(int fileId)
        {
            return await _fileRepository.GetById(fileId);
        }

        public async Task<FileRecord> GetByGuid(string guid)
        {
            return await _fileRepository.GetByGuid(guid);
        }

        public async Task<string> GetDownloadLink(int fileId)
        {
            var file = await GetById(fileId);

            return $"{_configuration["AppSettings:RootUrl"]}/api/files/{file.Guid}/download";
        }
    }
}
