using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using XplicityApp.Configurations;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;
using Xunit.Abstractions;
using XplicityApp.Dtos.Holidays;

namespace Tests.Tests.Holidays
{
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
        private readonly TimeService _timeService;

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
            _timeService = new TimeService();
            var mockUserService = new Mock<IUserService>().Object;
            var mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            _holidaysRepository = new HolidaysRepository(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);
            _clientsRepository = new ClientsRepository(_context);
            var holidayGuidsRepository = new HolidayGuidsRepository(_context);
            var mockNotificationSettingsService = new Mock<INotificationSettingsService>().Object;
            _employeesService = new EmployeesService(_employeesRepository, mapper, mockOvertimeUtility, _timeService,
                                                     mockUserService, mockNotificationSettingsService, _configuration);
            _holidaysService = new HolidaysService(_holidaysRepository, _employeesRepository, mapper, _timeService,
                                                   mockOvertimeUtility, _clientsRepository, mockUserService, _configuration, holidayGuidsRepository);
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
        public async void When_GettingHolidayEmployeeFullName_Expect_ReturnedEmployeeFullNameIsCorrect(int id)
        {
            var retrievedHoliday = await _holidaysService.GetById(id);
            var retrievedEmployee = await _employeesService.GetById(retrievedHoliday.EmployeeId);

            var fullNameExpected = $"{retrievedEmployee.Name} {retrievedEmployee.Surname}";
            var fullNameActual = retrievedHoliday.EmployeeFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Employee's full name is incorrect.");
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
    }
}

