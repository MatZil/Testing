﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        public FileService(IFileRepository fileRepository, IConfiguration configuration)
        {
            _fileRepository = fileRepository;
            _configuration = configuration;
        }

        private async Task<int> CreateFileRecord(IFormFile file, FileTypeEnum fileType)
        {
            var fileRecordToCreate = new FileRecord
            {
                Name = file.FileName,
                Type = fileType,
                CreatedAt = DateTime.Now
            };
            var fileId = await _fileRepository.Create(fileRecordToCreate);
            return fileId;
        }
        public async Task<string> Upload(IFormFile formFile, FileTypeEnum fileType)
        {
            if (formFile.Length > 0)
            {
                var fileId =  await CreateFileRecord(formFile, fileType);
                var filePath = BuildFilePath(await _fileRepository.GetById(fileId));

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

        private string BuildFilePath(FileRecord file)
        {
            string folderName = string.Empty;
            switch (file.Type)
            {
                case FileTypeEnum.HolidayPolicy:
                    folderName = Path.Combine(_configuration.GetValue<string>("FileConfig:HolidayPolicy"), file.Id.ToString());
                    break;
                case FileTypeEnum.Document:
                    folderName = Path.Combine(_configuration.GetValue<string>("FileConfig:Document"), file.Id.ToString());
                    break;
                case FileTypeEnum.Image: 
                    folderName = Path.Combine(_configuration.GetValue<string>("FileConfig:Image"), file.Id.ToString());
                    break;
            }
            return folderName;
        }
        public async Task<string> GetByType(FileTypeEnum fileType)
        {
            var file = await _fileRepository.FindByType(fileType);
            var folderName = BuildFilePath(file);
            var filePath = Path.Combine(folderName, file.Name).Replace(@"\", "/");
            return filePath;
        }
    }
}
