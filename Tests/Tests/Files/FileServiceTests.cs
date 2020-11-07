using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XplicityApp.Configurations;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;

namespace Tests.Tests.Files
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;
        private readonly Mock<IAzureStorageService> _azureStorageServiceMock;
        private readonly IConfiguration _configuration;
        private readonly HolidayDbContext _context;

        public FileServiceTests()
        {
            var setup = new SetUp();
            setup.Initialize();

            _context = setup.HolidayDbContext;
            _configuration = setup.GetConfiguration();
            _azureStorageServiceMock = new Mock<IAzureStorageService>();
            ConfigureAzureStorageServiceMock();
            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock
                .Setup(s => s.GetCurrentTime())
                .Returns(new DateTime(2020, 7, 9, 6, 0, 0))
                ;

            _fileService = new FileService(
                new FileRepository(_context),
                _configuration,
                timeServiceMock.Object, 
                _azureStorageServiceMock.Object
            );
        }

        private void ConfigureAzureStorageServiceMock()
        {
            _azureStorageServiceMock
                .Setup(s => s.GetBlobUrl(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string container, string blob) => $"baseUrl/{container}/{blob}?sv=12345")
                ;
        }

        [Theory]
        [InlineData("testName.png", FileTypeEnum.Image)]
        public async Task When_CreatingFileRecord_Expect_FileCreated(string fileName, FileTypeEnum fileType)
        {
            var id = await _fileService.CreateFileRecord(fileName, fileType);

            var createdFile = await _context.FileRecords.FirstAsync(fr => fr.Id == id);

            Assert.Equal(fileName, createdFile.Name);
            Assert.Equal(fileType, createdFile.Type);
            Assert.Equal(new DateTime(2020, 7, 9, 6, 0, 0), createdFile.CreatedAt);
        }

        [Theory]
        [InlineData(FileTypeEnum.Document)]
        public async Task When_UploadingFile_Expect_FileUploaded(FileTypeEnum fileType)
        {
            var testFormFile = GetTestFormFile();
            await _fileService.Upload(testFormFile, fileType);

            _azureStorageServiceMock.Verify(
                s => s.UploadBlob(_configuration["BlobConfig:Documents"], testFormFile.FileName, testFormFile.ContentType, It.IsAny<Stream>()),
                Times.Once
                );
        }

        private static FormFile GetTestFormFile() 
        {
            var testBytes = Encoding.UTF8.GetBytes("test file");

            return new FormFile(
                baseStream: new MemoryStream(testBytes),
                baseStreamOffset: 0,
                length: testBytes.Length,
                name: "Test",
                fileName: $"test-{Guid.NewGuid()}.txt"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain; charset=utf-8"
            };
        }

        [Fact]
        public void When_GettingNewestPolicyPath_Expect_PathReturned()
        {
            var actualPath = _fileService.GetHolidayPolicyPath();

            Assert.Equal($"baseUrl/policy/{_configuration["FileConfig:HolidayPolicyFileName"]}?sv=12345", actualPath);
            _azureStorageServiceMock.Verify(s => s.GetBlobUrl("policy", _configuration["FileConfig:HolidayPolicyFileName"]), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task When_GettingDownloadLink_Expect_CorrectLink(int fileId)
        {
            var actualLink = await _fileService.GetDownloadLink(fileId);

            Assert.Equal($"https://localhost:5001/api/files/{fileId}/download", actualLink);
        }

        private static BlobServiceClient GetBlobServiceClient()
        {
            var connectionString = AzureStorageConfiguration.GetConnectionString();

            return new BlobServiceClient(connectionString);
        }
    }
}
