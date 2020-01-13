using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.DocxGeneration;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Services;
using Xunit;

namespace Tests
{
    public class DocxGenerationTests
    {
        private readonly HolidayDbContext _context;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly EmployeesRepository _employeesRepository;
        private readonly DocxGeneratorService _docxGeneratorService;
        private readonly IConfiguration _config;
        private readonly TimeService _timeService;

        public DocxGenerationTests()
        {
            var setup = new SetUp();
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;
            _config = setup.GetConfiguration();
            var userManager = setup.InitializeUserManager(_context);

            _holidaysRepository = new HolidaysRepository(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            _timeService = new TimeService();
            var docxGenerationMock = new Mock<IDocxGenerator>();
            docxGenerationMock.Setup(generator => generator
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
                                            CreatedAt = _timeService.GetCurrentTime()
                                        }.Id));


            _docxGeneratorService = new DocxGeneratorService(docxGenerationMock.Object, _holidaysRepository, _employeesRepository);
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
                CreatedAt = _timeService.GetCurrentTime()
            }.Id;

            var actualId = await _docxGeneratorService.GenerateHolidayDocx(holidayId, documentType);

            Assert.True(expectedId == actualId, "DocxGeneration returned unexpected file record.");
        }


    }
}
