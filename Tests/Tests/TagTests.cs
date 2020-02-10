using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using XplicityApp.Dtos.Tags;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.TagTests.AlphabeticalOrderer", "Tests")]
    public class TagTests
    {
        private readonly HolidayDbContext _context;
        private readonly TagsService _tagsService;
        private readonly int _tagsCount;
        public TagTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            _tagsCount = setup.GetCount("tags");


            var mapper = setup.Mapper;
            var tagsRepository = new TagsRepository(_context);
            _tagsService = new TagsService(tagsRepository, mapper);
        }

        [Theory]
        [InlineData("ReallyLongTagTitle")]
        [InlineData("Ta")]
        [InlineData("Tag Tag")]
        [InlineData("Tag_Tag")]
        [InlineData("Tag!Tag")]
        [InlineData("")]
        public async Task When_CreateTag_Expect_ValidationFail(string tagTitle)
        {
            var tagDto = new NewTagDto
            {
                Title = tagTitle
            };

            await Assert.ThrowsAsync<ValidationException>(() => _tagsService.Create(tagDto));
        }

        [Theory]
        [InlineData("#Tag")]
        [InlineData("Tag-Tag")]
        [InlineData("Tag123")]
        public async void When_CreateTag_Expect_TagWasCreated(string tagTitle)
        {
            var tagDto = new NewTagDto
            {
                Title = tagTitle
            };

            Assert.True(await _tagsService.Create(tagDto) > 0);
        }

        [Fact]
        public async Task When_GetAllTags_Expect_OneOrMoreTagsWereFound()
        {
            var tagsCount = (await _tagsService.GetAll()).Count;

            Assert.True(tagsCount == _tagsCount);
        }

        [Theory]
        [InlineData(3)]
        public async void When_GetById_Expect_TagNotExists(int id)
        {
            var tagDto = await _tagsService.GetById(id);

            Assert.Null(tagDto);
        }

        [Theory]
        [InlineData("Tag", 2)]
        [InlineData("ag1", 1)]
        [InlineData("#", 0)]
        public async void When_FindByTitle_Expect_CountOfTagByTitle(string tagTitle, int expectedCount)
        {
            var tagsCount = (await _tagsService.FindByTitle(tagTitle)).Count;

            Assert.True(tagsCount == expectedCount);
        }
    }


}
