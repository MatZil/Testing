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
        [InlineData(FileTypeEnum.Document, "Resources/Documents")]
        [InlineData(FileTypeEnum.HolidayPolicy, "Resources/Policy")]
        [InlineData(FileTypeEnum.Image, "Resources/Images")]
        [InlineData(FileTypeEnum.Order, "Resources/Orders")]
        [InlineData(FileTypeEnum.Request, "Resources/Requests")]
        [InlineData(FileTypeEnum.Unknown, "Resources/Unknown")]
        public async void When_UploadingFile_Expect_FileUploaded(FileTypeEnum fileType, string filePath)
        {

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
            await _fileService.Upload(formFile, fileType);

            Assert.True(Directory.Exists(expectedDirectoryPath));
            Assert.True(File.Exists(expectedFilePath));


            File.Delete(expectedFilePath);
            Directory.Delete(expectedDirectoryPath);
        }
        [Fact]
        public async void When_GettingNewestPolicyPath_Expect_PathReturned()
        {
            var actualPath = await _fileService.GetNewestPolicyPath();
            var expectedPath = "Resources/Policy\\HolidayPolicy";
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
