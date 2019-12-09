using Xunit;
using Xplicity_Holidays.Services;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Repositories;
using AutoMapper;
using Xplicity_Holidays.Dtos.Holidays;
using Xunit.Abstractions;
using System;
using Xplicity_Holidays.Infrastructure.Utils;
using Xplicity_Holidays.Infrastructure.Enums;

namespace Tests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class HolidayTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _holidaysCount;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysService _holidaysService;
        private readonly HolidaysRepository _holidaysRepository;

        public HolidayTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize(out _context, out IMapper _mapper);
            _holidaysCount = setup.GetCount("holidays");

            var timeService = new TimeService();
            _holidaysRepository = new HolidaysRepository(_context);
            _holidaysService = new HolidaysService(_holidaysRepository, _mapper, timeService);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingExistingHolidayById_Expect_ReturnsHoliday(int id)
        {
            var retrievedHoliday = await _holidaysService.GetById(id);
            Assert.NotNull(retrievedHoliday);
        }

        [Theory]
        [InlineData(4)]
        public async void When_GettingNonexistentHolidayById_Expect_ReturnsNull(int id)
        {
            var retrievedHoliday = await _holidaysService.GetById(id);
            _output.WriteLine("Holiday by this id does not exist");

            Assert.Null(retrievedHoliday);
        }

        [Fact]
        public async void When_GettingAllHolidays_Expect_ReturnsAllHolidays()
        {
            var retrievedHolidays = await _holidaysService.GetAll();
            Assert.Equal(_holidaysCount, retrievedHolidays.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_CreatingHoliday_Expect_ReturnsCreatedHoliday(int employeeId)
        {
            var newHoliday = new NewHolidayDto()
            {
                EmployeeId = employeeId,
                Type = HolidayType.Parental,
                FromInclusive = new DateTime(2019, 10, 24),
                ToExclusive = new DateTime(2019, 10, 28),
            };

            int createdHolidayId = -1;
            createdHolidayId = await _holidaysService.Create(newHoliday);
            var createdHoliday = _context.Holidays.Find(createdHolidayId);

            Assert.True(createdHolidayId > -1 && createdHoliday != null);
        }

        //[Theory]
        //[InlineData(3)]
        //public void When_CreatingHolidayWithNonexistentEmployee_Expect_EmployeeException(int employeeId)
        //{
        //    var newHoliday = new NewHolidayDto()
        //    {
        //        EmployeeId = employeeId,
        //        Type = HolidayType.Parental,
        //        FromInclusive = new DateTime(2019, 10, 24),
        //        ToExclusive = new DateTime(2019, 10, 28),
        //    };

        //    var exception = Record.ExceptionAsync(async () => await _holidaysService.Create(newHoliday));
        //    _output.WriteLine(exception.Result.Message);

        //    Assert.Equal("Employee not found", exception.Result.Message);
        //}

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_DeletingHoliday_Expect_True(int id)
        {
            var wasFound = false;
            var wasDeleted = false;

            var found = _context.Holidays.Find(id);
            if (found != null) wasFound = true;

            bool deletedHoliday = await _holidaysService.Delete(id);

            found = _context.Holidays.Find(id);

            if (found == null && deletedHoliday) wasDeleted = true;

            Assert.True(wasFound && wasDeleted);
        }

        [Theory]
        [InlineData(4)]
        public async void When_DeletingNonexistentHoliday_Expect_False(int id)
        {
            _output.WriteLine("Couldn't find holiday to delete");
            bool deletedHoliday = await _holidaysService.Delete(id);

            Assert.False(deletedHoliday);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingHoliday_Expect_UpdatesHoliday(int id)
        {
            var initial = _context.Holidays.Find(id).FromInclusive;

            var updatedHoliday = new UpdateHolidayDto()
            {
                FromInclusive = new DateTime(2222, 02, 22),
            };
            var expected = updatedHoliday.FromInclusive;

            await _holidaysService.Update(id, updatedHoliday);
            var actual = _context.Holidays.Find(id).FromInclusive;
            _output.WriteLine(initial + "   >>   " + actual);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        public void When_UpdatingNonexistentHoliday_Expect_InvalidOperationException(int id)
        {
            var updatedHoliday = new UpdateHolidayDto()
            {
                FromInclusive = new DateTime(2222, 02, 22),
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _holidaysService.Update(id, updatedHoliday));
        }

        //[Theory]
        //[InlineData(1, 3)]
        //public void When_UpdatingHolidayWithNonexistentEmployee_Expect_EmployeeException(int holidayId, int employeeId)
        //{
        //    var updatedHoliday = new UpdateHolidayDto()
        //    {
        //        EmployeeId = employeeId,
        //    };

        //    var exception = Record.ExceptionAsync(async () => await _holidaysService.Update(holidayId, updatedHoliday));
        //    _output.WriteLine(exception.Result.Message);

        //    Assert.Equal("Employee not found", exception.Result.Message);
        //}
    }
}
