using System;
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
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using XplicityApp.Configurations;
using XplicityApp.Services.Interfaces;
using Xunit.Abstractions;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.FileServiceTests.AlphabeticalOrderer", "Tests")]
    public class FileServiceTests
    {
        private readonly ITestOutputHelper _output;
        private readonly HolidayDbContext _context;
        private readonly FileService _fileService;
        private readonly FileRepository _fileRepository;
        private readonly IConfiguration _configuration;
        private readonly ITimeService _timeService;
        private readonly IAzureStorageService _azureStorageService;
        public FileServiceTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;

            _timeService = new Mock<ITimeService>().Object;
            _fileRepository = new FileRepository(_context);
            _configuration = setup.GetConfiguration();
            _azureStorageService = new AzureStorageService();

            _fileService = new FileService(
                _fileRepository,
                _configuration,
                _timeService,
                _azureStorageService
            );
        }

        [Theory]
        [InlineData(FileTypeEnum.Document)]
        [InlineData(FileTypeEnum.HolidayPolicy)]
        [InlineData(FileTypeEnum.Image)]
        [InlineData(FileTypeEnum.Order)]
        [InlineData(FileTypeEnum.Request)]
        [InlineData(FileTypeEnum.Unknown)]
        public async void When_UploadingFile_Expect_FileUploaded(FileTypeEnum fileType)
        {
            var testBytes = Encoding.UTF8.GetBytes("test file");
            var azureStorageService = new AzureStorageService();

            var formFile = new FormFile(
                baseStream: new MemoryStream(testBytes),
                baseStreamOffset: 0,
                length: testBytes.Length,
                name: "Test",
                fileName: "test.txt"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain; charset=utf-8"
            };
            var connectionString = AzureStorageConfiguration.GetConnectionString();
            _output.WriteLine($"Connection str: {connectionString}"); // todo remove
            _output.WriteLine($"Config str: {_configuration.GetValue<string>("AzureStorage:EnvironmentVariableName")}");
            _output.WriteLine($"Env variable: {Environment.GetEnvironmentVariable("XplicityAppStorageConnection")}"); // todo remove
            _output.WriteLine($"Env variable process: {Environment.GetEnvironmentVariable("XplicityAppStorageConnection", EnvironmentVariableTarget.Process)}"); // todo remove
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var containerName = _fileService.GetBlobContainerName(fileType);
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient;
            if (!container.Exists())
            {
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            else
            {
                containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }

            await using (var fileStream = formFile.OpenReadStream())
            {
                await azureStorageService.UploadBlob(containerName, formFile.FileName, formFile.ContentType,
                    fileStream);
            }

            var blobClientTest = containerClient.GetBlobClient(formFile.FileName);

            Assert.True(await blobClientTest.ExistsAsync());
            var downloadInfo = await azureStorageService.GetBlobDownloadInfo(containerName, formFile.FileName);
            Assert.NotNull(downloadInfo);
            Assert.Equal(formFile.ContentType, downloadInfo.ContentType);

            await blobClientTest.DeleteIfExistsAsync();
        }

        [Fact]
        public void When_GettingNewestPolicyPath_Expect_PathReturned()
        {
            string connectionString = AzureStorageConfiguration.GetConnectionString();
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            var actualPath = _fileService.GetNewestPolicyPath();
            var expectedPath = new Uri(blobServiceClient.Uri, "/policy/Holiday%20Policy.pdf").AbsoluteUri;
            Assert.StartsWith(expectedPath, actualPath);
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
