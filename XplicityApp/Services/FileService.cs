﻿using Microsoft.AspNetCore.Http;
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
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            //string containerName = GetRelativeDirectory(fileType);
            string containerName = "policy";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);


            await CreateFileRecord(formFile.FileName, fileType);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), GetRelativeDirectory(fileType), formFile.FileName);

            BlobClient blobClient = containerClient.GetBlobClient(formFile.FileName);

            using FileStream uploadFileStream = File.OpenRead(fullPath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            //await CreateFileRecord(formFile.FileName, fileType);
            //var fullPath = Path.Combine(Directory.GetCurrentDirectory(), GetRelativeDirectory(fileType), formFile.FileName);
            //using var fileStream = new FileStream(fullPath, FileMode.Create);
            //formFile.CopyTo(fileStream);
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
        public async Task<string> GetNewestPolicyPath()
        {
            var policy = await _fileRepository.GetNewestPolicy();
            return Path.Combine(GetRelativeDirectory(policy.Type), policy.Name);
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
