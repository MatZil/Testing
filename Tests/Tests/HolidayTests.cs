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
using Microsoft.Extensions.Configuration;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class HolidayTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _holidaysCount;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysService _holidaysService;
        private readonly EmployeesService _employeesService;
        private readonly EmployeesRepository _employeesRepository;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly ClientsRepository _clientsRepository;
        private readonly IConfiguration _configuration;

        public HolidayTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            var mapper = setup.Mapper;
            var userManager = setup.InitializeUserManager();
            _holidaysCount = setup.GetCount("holidays");
            _configuration = setup.GetConfiguration();

            var timeService = new TimeService();
            var mockUserService = new Mock<IUserService>().Object;
            var mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            _holidaysRepository = new HolidaysRepository(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            _clientsRepository = new ClientsRepository(_context);
            var mockNotificationSettingsService = new Mock<INotificationSettingsService>().Object;
            _employeesService = new EmployeesService(_employeesRepository, mapper, mockOvertimeUtility, timeService, 
                                                     mockUserService, mockNotificationSettingsService);
            _holidaysService = new HolidaysService(_holidaysRepository, _employeesRepository, mapper, timeService, 
                                                   mockOvertimeUtility, _clientsRepository, mockUserService, _configuration);
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
        public void When_PublicHolidaysIsUnpaid_Expect_ThisDayIsUnpaid(int id, bool expected)
        {
            var result = _employeesService.HasActiveUnpaidHoliday(id);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
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
                Type = HolidayType.DayForChildren,
                FromInclusive = new DateTime(2019, 10, 24),
                ToInclusive = new DateTime(2019, 10, 27),
            };

            var createdHolidayId = await _holidaysService.Create(newHoliday);
            var createdHoliday = _context.Holidays.Find(createdHolidayId);

            Assert.True(createdHolidayId > -1 && createdHoliday != null);
        }

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
        [InlineData(-1)]
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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingHolidayEmployeeFullName_Expect_ReturnedEmployeeFullNameIsCorrect(int id)
        {
            var retrievedHoliday = await _holidaysService.GetById(id);
            var retrievedEmployee = await _employeesService.GetById(retrievedHoliday.EmployeeId);

            var fullNameExpected = $"{retrievedEmployee.Name} {retrievedEmployee.Surname}";
            var fullNameActual = retrievedHoliday.EmployeeFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Employee's full name is incorrect.");
        }

        [Theory]
        [InlineData(EmployeeStatusEnum.Current)]
        [InlineData(EmployeeStatusEnum.Former)]
        public async void When_GettingHolidaysByEmployeeStatus_Expect_ReturnsHolidays(EmployeeStatusEnum employeeStatus)
        {
            var retrievedHolidays = await _holidaysService.GetByEmployeeStatus(employeeStatus);
            int actualHolidaysCount = 0;

            var allHolidays = await _holidaysService.GetAll();
            foreach (var holiday in allHolidays)
            {
                var employee = await _employeesService.GetById(holiday.EmployeeId);
                if(employee.Status == employeeStatus)
                {
                    actualHolidaysCount++;
                }
            }
            Assert.Equal(retrievedHolidays.Count, actualHolidaysCount);
        }

        [Fact]
        public async void When_GettingConfirmedHolidaysByMonth_Expect_ReturnsHolidaysStartingFromEndOfLastMonth()
        {
            var currentMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var numberOfLastMonthDays = _configuration.GetValue<int>("CalendarConfig:NumberOfLastMonthDays");
            var startDate = currentMonthFirstDay.AddDays(-numberOfLastMonthDays);
            _output.WriteLine(startDate.ToString());
            var selectedDate = DateTime.Today;
            var selectedMonthConfirmedHolidays = await _holidaysService.GetConfirmedByMonth(selectedDate, 1);

            foreach (var holiday in selectedMonthConfirmedHolidays)
            {
                _output.WriteLine(holiday.FromInclusive.ToString() + "  " + holiday.ToInclusive.ToString());
                Assert.True(holiday.ToInclusive >= startDate && holiday.Status == HolidayStatus.AdminConfirmed);
            }
        }

        [Fact]
        public async void When_GettingConfirmedHolidaysByMonth_Expect_ReturnsHolidaysLastingToStartOfNextMonth()
        {
            var currentMonthFirstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var numberOfNextMonthDays = _configuration.GetValue<int>("CalendarConfig:NumberOfNextMonthDays");
            var endDate = currentMonthFirstDay.AddMonths(1).AddDays(numberOfNextMonthDays - 1);

            _output.WriteLine(endDate.ToString());
            var selectedDate = DateTime.Today;
            var selectedMonthConfirmedHolidays = await _holidaysService.GetConfirmedByMonth(selectedDate, 1);

            foreach (var holiday in selectedMonthConfirmedHolidays)
            {
                _output.WriteLine(holiday.FromInclusive.ToString() + "  " + holiday.ToInclusive.ToString());
                Assert.True(holiday.FromInclusive <= endDate && holiday.Status == HolidayStatus.AdminConfirmed);
            }
        }
    }
}