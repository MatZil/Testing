using AutoMapper;
using Moq;
using System;
using Microsoft.Extensions.Logging;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;
using XplicityApp.Services.Extensions;
using XplicityApp.Services.Validations;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.HolidayConfirmTests.AlphabeticalOrderer", "Tests")]
    public class HolidayConfirmTests
    {
        private readonly HolidayDbContext _context;
        private readonly HolidayConfirmService _holidayConfirmService;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        private readonly EmployeesRepository _employeesRepository;
        private readonly TimeService _timeService;
        private readonly IOvertimeUtility _mockOvertimeUtility;
        private readonly EmployeeHolidaysConfirmationUpdater _employeeHolidaysConfirmationUpdater;
        private readonly HolidayValidationService _holidayValidationService;

        public HolidayConfirmTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            _mapper = setup.Mapper;

            _timeService = new Mock<TimeService>().Object;
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = setup.InitializeUserManager();
            _employeesRepository = new EmployeesRepository(_context, userManager);
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            var mockEmailService = new Mock<IEmailService>();
            var mockDocxGeneratorService = new Mock<IDocxGeneratorService>();
            _mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            _employeeHolidaysConfirmationUpdater = new EmployeeHolidaysConfirmationUpdater(_employeesRepository, _timeService, _mockOvertimeUtility);

            var holidaysService = new HolidaysService(_holidaysRepository, _mapper, _timeService, _mockOvertimeUtility);
            _holidayConfirmService = new HolidayConfirmService(mockEmailService.Object, _mapper, _holidaysRepository,
                                                               _employeesRepository, clientsRepository, holidaysService,
                                                                mockDocxGeneratorService.Object, _mockOvertimeUtility, 
                                                               _employeeHolidaysConfirmationUpdater, new Mock<ILogger<HolidayConfirmService>>().Object);
            _holidayValidationService = new HolidayValidationService(
                _holidaysRepository,
                _employeesRepository,
                _mapper,
                _timeService);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_RequestingClientApproval_Expect_True(int holidayId)
        {
            var result = await _holidayConfirmService.RequestClientApproval(holidayId);

            Assert.True(result, "Request for client's approval was successfully submitted.");
        }

        //[Theory]
        //[InlineData(1, "status")]
        //public async void When_RequestingAdminApproval_Expect_True(int holidayId, string clientStatus)
        //{
        //    var result = await _holidayConfirmService.RequestAdminApproval(holidayId, clientStatus);
        //    Assert.True(result);
        //}

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_ConfirmingHoliday_Expect_True(int holidayId)
        {
            await _holidayConfirmService.ConfirmHoliday(holidayId);

            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status == "Confirmed", "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void When_UpdatingEmployeesWorkdays_Expect_UpdatesWorkdays(int holidayId)
        {
            var index = _context.Holidays.Find(holidayId).EmployeeId;
            var employee = await _employeesRepository.GetById(index);
            var initial = employee.FreeWorkDays;
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var workdays = _timeService.GetWorkDays(holidayDto.FromInclusive, holidayDto.ToInclusive);
            var expected = initial - workdays + holiday.OvertimeDays;

            await _employeeHolidaysConfirmationUpdater.UpdateEmployeesWorkdays(holidayDto);

            employee = await _employeesRepository.GetById(index);
            var actual = employee.FreeWorkDays;

            Assert.True(expected == actual, "Failed to update employee's free workdays.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void When_UpdatingEmployeesOvertime_Expect_UpdatesOvertime(int holidayId)
        {
            var index = _context.Holidays.Find(holidayId).EmployeeId;
            var employee = await _employeesRepository.GetById(index);
            var initial = employee.OvertimeHours;
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var expected = initial - _mockOvertimeUtility.ConvertOvertimeDaysToHours(holiday.OvertimeDays);

            await _employeeHolidaysConfirmationUpdater.UpdateEmployeesOvertime(holidayDto);

            employee = await _employeesRepository.GetById(index);
            var actual = employee.OvertimeHours;

            Assert.True(expected == actual, "Failed to update employee's overtime hours.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingParentalLeaves_Expect_UpdatesParentalLeaves(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var employee = await _employeesRepository.GetById(holidayDto.EmployeeId);
            var initial = new int[2];
            initial[0] = employee.CurrentAvailableLeaves;
            initial[1] = employee.NextMonthAvailableLeaves;

            await _employeeHolidaysConfirmationUpdater.UpdateParentalLeaves(holidayDto);

            var final = new int[2];
            final[0] = employee.CurrentAvailableLeaves;
            final[1] = employee.NextMonthAvailableLeaves;

            Assert.True(initial[0] != final[0] || initial[1] != final[1], "Failed to update employee's parental leaves.");
        }

        [Theory]
        [InlineData(4)]
        public async void When_ConfirmingInvalid_Expect_False(int holidayId)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _holidayValidationService.ValidateHolidayConfirmationReadiness(holidayId));
        }
    }
}
