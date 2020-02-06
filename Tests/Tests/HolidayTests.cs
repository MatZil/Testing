using Xunit;
using XplicityApp.Services;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Dtos.Holidays;
using Xunit.Abstractions;
using System;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Infrastructure.Enums;
using Moq;
using XplicityApp.Services.Interfaces;
using XplicityApp.Infrastructure.Utils;

namespace Tests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class HolidayTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _holidaysCount;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysService _holidaysService;
        private readonly ITimeService mockTimeService;
        private readonly EmployeesService _employeesService;
        private readonly EmployeesRepository _employeesRepository;

        public HolidayTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            var mapper = setup.Mapper;
            var userManager = setup.InitializeUserManager();
            _holidaysCount = setup.GetCount("holidays");

            var timeService = new TimeService();
            mockTimeService = new Mock<ITimeService>().Object;
            var mockUserService = new Mock<IUserService>().Object;
            var mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            var holidaysRepository = new HolidaysRepository(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            _holidaysService = new HolidaysService(holidaysRepository, mapper, mockTimeService, mockOvertimeUtility);
            _employeesService = new EmployeesService(_employeesRepository, mapper, mockOvertimeUtility, timeService, mockUserService);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingExistingHolidayById_Expect_ReturnsHoliday(int id)
        {
            var retrievedHoliday = await _holidaysService.GetById(id);
            Assert.NotNull(retrievedHoliday);
        }

        [Fact]
        public void When_GetPublicHolidays_Expect_DaysCountWithoutHolidays()
        {
            var timeService = new TimeService();
            var retrievedHolidays = timeService.GetWorkDays(new DateTime(2020, 12, 23), new DateTime(2020, 12, 27));

            Assert.Equal(1, retrievedHolidays);
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(1, false)]
        public async void When_PublicHolidaysIsUnpaid_Expect_ThisDayIsUnpaid(int id, bool expected)
        {
            var result = await _employeesService.HasActiveUnpaidHoliday(id);

            Assert.Equal(expected, result);
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
                ToInclusive = new DateTime(2019, 10, 28),
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
