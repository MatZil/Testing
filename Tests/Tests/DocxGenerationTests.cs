using AutoMapper;
using Moq;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.DocxGeneration;
using Xplicity_Holidays.Services;
using Xunit;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Infrastructure.Utils;
using Xplicity_Holidays.Infrastructure.Database.Models;
using System.Threading.Tasks;

namespace Tests.Tests
{
    public class DocxGenerationTests
    {
        private readonly Set_up _setup;
        private readonly HolidayDbContext _context;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly EmployeesRepository _employeesRepository;
        private readonly DocxGeneratorService _docxGeneratorService;
        private readonly IConfiguration _config;
        private readonly TimeService _timeService;

        public DocxGenerationTests()
        {
            _setup = new Set_up();
            _setup.Initialize(out _context, out IMapper mapper);
            _config = _setup.GetConfiguration();
            var userManager = _setup.InitializeUserManager(_context);

            _holidaysRepository = new HolidaysRepository(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            _timeService = new TimeService();
            var docxGenerationMock = new Mock<IDocxGenerator>();
            docxGenerationMock.Setup(generator => generator
                                    .GenerateDocx(It.IsAny<Holiday>(), It.IsAny<Employee>(), It.IsAny<HolidayDocumentType>())).Returns(
                                    (Holiday holiday, Employee employee, HolidayDocumentType documentType) => 
                                    Task.FromResult(Path.Combine(_config["DocxGeneration:GenerationDir"],
                                    $"{holiday.Id}-{documentType.ToString()}{holiday.Type.ToString()}" +
                                    $"-{_timeService.GetCurrentTime().ToString("yyyy-MM-dd")}.docx")));


            _docxGeneratorService = new DocxGeneratorService(docxGenerationMock.Object, _holidaysRepository, _employeesRepository);
        }

        [Theory]
        [InlineData(1, HolidayDocumentType.Order)]
        [InlineData(2, HolidayDocumentType.Request)]
        public async void When_GeneratingDocx_Expect_FilePath(int holidayId, HolidayDocumentType documentType)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var expectedPath = Path.Combine(_config["DocxGeneration:GenerationDir"],
                       $"{holidayId}-{documentType.ToString()}{holiday.Type.ToString()}" +
                       $"-{_timeService.GetCurrentTime().ToString("yyyy-MM-dd")}.docx");
            
            

            var actualPath = await _docxGeneratorService.GenerateHolidayDocx(holidayId, documentType);

            Assert.Equal(expectedPath, actualPath);
        }
    }
}
