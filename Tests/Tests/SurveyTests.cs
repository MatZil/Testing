using Microsoft.Extensions.Configuration;
using System;
using XplicityApp.Dtos.Surveys;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.SurveyTests.AlphabeticalOrderer", "Tests")]
    public class SurveyTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _surveysCount;
        private readonly SurveysService _surveysService;
        private readonly IConfiguration _configuration;

        public SurveyTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _configuration = setup.GetConfiguration();
            _context = setup.HolidayDbContext;
            var mapper = setup.Mapper;
            _surveysCount = setup.GetCount("surveys");
            ISurveysRepository surveysRepository = new SurveysRepository(_context);
            _surveysService = new SurveysService(surveysRepository, _configuration, mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingExistingSurveyById_Expect_ReturnsSurvey(int id)
        {
            var retrievedSurvey = await _surveysService.GetById(id);

            Assert.NotNull(retrievedSurvey);
        }

        [Theory]
        [InlineData(-1)]
        public async void When_GettingNonexistentSurveyById_Expect_ReturnsNull(int id)
        {
            var retrievedSurvey = await _surveysService.GetById(id);

            Assert.Null(retrievedSurvey);
        }

        [Fact]
        public async void When_GettingAllSurveys_Expect_ReturnsAllSurveys()
        {
            var retrievedSurveys = await _surveysService.GetAll();

            Assert.Equal(_surveysCount, retrievedSurveys.Count);
        }

        [Fact]
        public async void When_CreatingSurvey_Expect_ReturnsCreatedSurvey()
        {
            var newSurvey = new NewSurveyDto()
            {
                Title = "titleNew"
            };

            var createdSurvey = await _surveysService.Create(newSurvey);

            Assert.NotNull(createdSurvey);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_DeletingSurvey_Expect_True(int id)
        {
            var wasFound = false;
            var wasDeleted = false;

            var found = _context.Surveys.Find(id);
            if (found != null)
            {
                wasFound = true;
            }

            bool deletedSurvey = await _surveysService.Delete(id);

            found = _context.Surveys.Find(id);
            if (found == null && deletedSurvey)
            {
                wasDeleted = true;
            }

            Assert.True(wasFound && wasDeleted);
        }

        [Theory]
        [InlineData(-1)]
        public async void When_DeletingNonexistentSurvey_Expect_False(int id)
        {
            bool deletedSurvey = await _surveysService.Delete(id);

            Assert.False(deletedSurvey);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingSurvey_Expect_ReturnsUpdatedSurvey(int id)
        {
            var initial = _context.Surveys.Find(id).Title;

            UpdateSurveyDto updatedSurvey = new UpdateSurveyDto()
            {
                Title = "UpdatedTitle",
            };
            var expected = updatedSurvey.Title;

            await _surveysService.Update(id, updatedSurvey);
            var actual = _context.Surveys.Find(id).Title;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1)]
        public void When_UpdatingNonexistentEmployee_Expect_InvalidOperationException(int id)
        {
            UpdateSurveyDto updatedSurvey = new UpdateSurveyDto()
            {
                Title = "UpdatedTitle",
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _surveysService.Update(id, updatedSurvey));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        public async void When_GettingExistingSurveyByGuid_Expect_ReturnsSurvey(string id)
        {
            var retrievedSurvey = await _surveysService.GetByGuid(id);

            Assert.NotNull(retrievedSurvey);
        }
    }
}
