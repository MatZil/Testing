using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using Azure.Storage.Blobs;
using System.Threading;
using System.Runtime.InteropServices;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace XplicityApp.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;

        public FileService(IFileRepository fileRepository, IConfiguration configuration, ITimeService timeService)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
            _timeService = timeService;
        }

        public async Task<int> CreateFileRecord(string fileName, FileTypeEnum fileType)
        {
            var guid = Guid.NewGuid().ToString() + '-' + Guid.NewGuid().ToString();

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

                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                string containerName = GetRelativeBlob(fileType);
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(formFile.FileName);
                
                if (fileType == FileTypeEnum.HolidayPolicy)
                {
                    blockBlob.Properties.ContentType = "application/pdf";
                }

                Stream fileStream = formFile.OpenReadStream();
                await blockBlob.UploadFromStreamAsync(fileStream);
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
        public string GetRelativeBlob(FileTypeEnum fileType)
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
        public string GetNewestPolicyPath()
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("policy");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("Holiday Policy.pdf");

            var policy = blockBlob.Uri.AbsoluteUri;
            return policy;
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
