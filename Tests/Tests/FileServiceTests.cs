using Microsoft.AspNetCore.Http;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.IO;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Moq;
using System.Text;
using XplicityApp.Infrastructure.Database.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Azure.Storage.Blobs.Specialized;
using System;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.FileServiceTests.AlphabeticalOrderer", "Tests")]
    public class FileServiceTests
    {
        private readonly HolidayDbContext _context;
        private readonly FileService _fileService;
        private readonly FileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;
        public FileServiceTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;

            _timeService = new Mock<ITimeService>().Object;
            _fileRepository = new FileRepository(_context);
            _configuration = setup.GetConfiguration();

            _fileService = new FileService(
                _fileRepository,
                _configuration,
                _timeService
            );
        }
        [Theory]
        [InlineData(FileTypeEnum.Document, "Resources/Documents", "documents")]
        [InlineData(FileTypeEnum.HolidayPolicy, "Resources/Policy", "policy")]
        [InlineData(FileTypeEnum.Image, "Resources/Images", "images")]
        [InlineData(FileTypeEnum.Order, "Resources/Orders", "orders")]
        [InlineData(FileTypeEnum.Request, "Resources/Requests", "requests")]
        [InlineData(FileTypeEnum.Unknown, "Resources/Unknown", "unknown")]
        public async void When_UploadingFile_Expect_FileUploaded(FileTypeEnum fileType, string filePath, string blobPath)
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            var formFile = new FormFile(
                new MemoryStream(Encoding.UTF8.GetBytes(fileType.ToString())),
                0,
                30,
                "Test",
                "test.txt"
            );
            var expectedFilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath, formFile.FileName);
            var expectedDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (File.Exists(expectedFilePath))
                File.Delete(expectedFilePath);

            if (Directory.Exists(expectedDirectoryPath))
                Directory.Delete(expectedDirectoryPath);

            Directory.CreateDirectory(expectedDirectoryPath);

            using var fileStream = new FileStream(expectedFilePath, FileMode.Create);
            formFile.CopyTo(fileStream);
            fileStream.Close();

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClientCheck = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClientCheck.GetContainerReference(blobPath);
            BlobContainerClient containerClient;
            
            if (!container.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(blobPath);
            }
            else
            {
                containerClient = blobServiceClient.GetBlobContainerClient(blobPath);
            }
            await _fileService.Upload(formFile, fileType);
            BlobClient blobClient = containerClient.GetBlobClient("test.txt");
            Assert.True(blobClient.Exists());
            blobClient.DeleteIfExists();
            //containerClient.DeleteIfExists();

            File.Delete(expectedFilePath);
            Directory.Delete(expectedDirectoryPath);
        }
        [Fact]
        public async void When_GettingNewestPolicyPath_Expect_PathReturned()
        {
                FileRecord expectedRecord = new FileRecord
                {
                    Name = "HolidayPolicy" 
                };

            var actualPath = await _fileService.GetNewestPolicyPath();
            var expectedPath = Path.Combine("Resources/Policy", expectedRecord.Name);
            Assert.Equal(expectedPath, actualPath);
        }
        [Theory]
        [InlineData(1, "Order")]
        [InlineData(2, "Request")]
        [InlineData(3, "HolidayPolicy")]
        public async void When_GettingById_Expect_FileRecordReturned(int fileId, string expectedName)
        {
            var actualFileRecord = await _fileService.GetById(fileId);
            Assert.Equal(expectedName, actualFileRecord.Name);
        }
    }
}
