using AutoMapper;
using Moq;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.TemplateGeneration;
using Xplicity_Holidays.Services;
using Xunit;

namespace Tests.Tests
{
    public class TemplateGenerationTests
    {
        private readonly Set_up _setup;
        private readonly HolidayDbContext _context;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly TemplateGenerationService _templateGenerationService;

        public TemplateGenerationTests()
        {
            _setup = new Set_up();
            _setup.Initialize(out _context, out IMapper mapper);

            _holidaysRepository = new HolidaysRepository(_context);
            var templateGeneration = new Mock<ITemplateGeneration>();

            _templateGenerationService = new TemplateGenerationService(templateGeneration.Object, _holidaysRepository);
        }

        [Theory]
        [InlineData(1, HolidayDocumentType.Order)]
        [InlineData(2, HolidayDocumentType.Request)]
        public async void When_GeneratingDocx_Expect_True(int holidayId, HolidayDocumentType documentType)
        {
            var result = await _templateGenerationService.GenerateHolidayDocx(holidayId, documentType);

            Assert.True(result);
        }
    }
}
