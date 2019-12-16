using System;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using AutoMapper;
using Xunit.Abstractions;


namespace Tests
{
    [TestCaseOrderer("Tests.EmailTemplateTests.AlphabeticalOrderer", "Tests")]
    public class EmailTemplateTests
    {
        private readonly HolidayDbContext _context;
        private readonly EmailTemplatesService _templatesService;
        private readonly SetUp _setup;
        private readonly ITestOutputHelper _output;
        private readonly int _templatesCount;
        private readonly IEmailTemplatesRepository _templatesRepository;

        public EmailTemplateTests(ITestOutputHelper output)
        {
            _output = output;
            _setup = new SetUp();
            var contextMapperTupple = _setup.Initialize();
            _context = contextMapperTupple.Item1;
            var _mapper = contextMapperTupple.Item2;

            _templatesCount = _setup.GetCount("emailTemplates");

            _templatesRepository = new EmailTemplatesRepository(_context);
            _templatesService = new EmailTemplatesService(_templatesRepository, _mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingTemplateById_Expect_ReturnsTemplate(int id)
        {
            var retrievedTemplate = await _templatesService.GetById(id);
            Assert.NotNull(retrievedTemplate);
        }

        [Theory]
        [InlineData(3)]
        public async void When_GettingNonexistentTemplateById_Expect_ReturnsNull(int id)
        {
            var retrievedTemplate = await _templatesService.GetById(id);
            _output.WriteLine("Template by this id does not exist");

            Assert.Null(retrievedTemplate);
        }

        [Fact]
        public async void When_GettingAllTemplates_Expect_ReturnsAllTemplates()
        {
            var retrievedTemplates = await _templatesService.GetAll();
            Assert.Equal(_templatesCount, retrievedTemplates.Count);
        }

        [Fact]
        public async void When_CreatingTemplate_Expect_ReturnsNewTemplate()
        {
            var newEmailTemplate = _setup.NewEmailTemplateDto();

            var createdTemplate = await _templatesService.Create(newEmailTemplate);

            Assert.NotNull(createdTemplate);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_DeletingTemplate_Expect_True(int id)
        {
            var wasFound = false;
            var wasDeleted = false;

            var found = _context.EmailTemplates.Find(id);
            if (found != null) wasFound = true;

            bool deletedTemplate = await _templatesService.Delete(id);

            found = _context.EmailTemplates.Find(id);

            if (found == null && deletedTemplate) wasDeleted = true;

            Assert.True(wasFound && wasDeleted);
        }

        [Theory]
        [InlineData(3)]
        public async void When_DeletingNonexistentTemplate_Expect_False(int id)
        {
            _output.WriteLine("Couldn't find template to delete");
            bool deletedTemplate = await _templatesService.Delete(id);

            Assert.False(deletedTemplate);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingTemplate_Expect_UpdatesTemplate(int id)
        {
            var initial = _context.EmailTemplates.Find(id).Template;

            var updatedEmailTemplate = _setup.NewEmailTemplateDto();
            var expected = updatedEmailTemplate.Template;

            await _templatesService.Update(id, updatedEmailTemplate);
            var actual = _context.EmailTemplates.Find(id).Template;
            _output.WriteLine(initial + "   >>   " + actual);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        public void When_UpdatingNonexistentTemplate_Expect_InvalidOperationException(int id)
        {
            var updatedEmailTemplate = _setup.NewEmailTemplateDto();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _templatesService.Update(id, updatedEmailTemplate));
        }
    }
}
