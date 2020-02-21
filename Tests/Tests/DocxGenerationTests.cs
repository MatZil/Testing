using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.DocxGeneration;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Xunit;

namespace Tests.Tests
{
    public class DocxGenerationTests
    {
        private readonly HolidaysRepository _holidaysRepository;
        private readonly DocxGeneratorService _docxGeneratorService;
        private readonly IConfiguration _config;
        private readonly ITimeService _mockTimeService;

        public DocxGenerationTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            _config = setup.GetConfiguration();
            var userManager = setup.InitializeUserManager();

            _holidaysRepository = new HolidaysRepository(context);
            var employeesRepository = new EmployeesRepository(context, userManager);
            _mockTimeService = new Mock<ITimeService>().Object;
            var mockDocxGenerationMock = new Mock<IDocxGenerator>();
            mockDocxGenerationMock.Setup(generator => generator
                                    .GenerateDocx(It.IsAny<Holiday>(), It.IsAny<Employee>(), It.IsAny<FileTypeEnum>())).Returns(
                                    (Holiday holiday, Employee employee, FileTypeEnum documentType) =>
                                    Task.FromResult(
                                        new FileRecord
                                        {
                                            Name = _config["DocxGeneration:NameFormat"]
                                                .Replace("{holidayId}", holiday.Id.ToString())
                                                .Replace("{documentType}", documentType.ToString())
                                                .Replace("{holidayType}", holiday.Type.ToString()),
                                            Type = documentType,
                                            CreatedAt = _mockTimeService.GetCurrentTime()
                                        }.Id));


            _docxGeneratorService = new DocxGeneratorService(mockDocxGenerationMock.Object, _holidaysRepository, employeesRepository);
        }

        [Theory]
        [InlineData(1, FileTypeEnum.Order)]
        [InlineData(2, FileTypeEnum.Request)]
        public async void When_GeneratingDocx_Expect_FilePath(int holidayId, FileTypeEnum documentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);

            var expectedId = new FileRecord
            {
                Name = _config["DocxGeneration:NameFormat"]
                        .Replace("{holidayId}", holidayId.ToString())
                        .Replace("{documentType}", documentType.ToString())
                        .Replace("{holidayType}", holiday.Type.ToString()),
                Type = documentType,
                CreatedAt = _mockTimeService.GetCurrentTime()
            }.Id;

            var actualId = await _docxGeneratorService.GenerateHolidayDocx(holidayId, documentType);

            Assert.True(expectedId == actualId, "DocxGeneration returned unexpected file record.");
        }


    }
}
