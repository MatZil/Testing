using AutoMapper;
using Moq;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.DocxGeneration;
using Xplicity_Holidays.Services;
using Xunit;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Utils;
using Xplicity_Holidays.Infrastructure.Database.Models;
using System.Threading.Tasks;

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
            var contextMapperTupple = setup.Initialize();
            _context = contextMapperTupple.Item1;
            var mapper = contextMapperTupple.Item2;
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
