using AutoMapper;
using Moq;
using Microsoft.Extensions.Logging;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;
using XplicityApp.Services.Extensions;

namespace Tests.Tests
{
    public class HolidayOvertimeTests
    {
        private readonly HolidayConfirmService _holidayConfirmService;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly IMapper _mapper;
        private readonly EmployeesRepository _employeesRepository;
        private readonly TimeService _mockTimeService;
        private readonly EmployeeHolidaysConfirmationUpdater _employeeHolidaysConfirmationUpdater;
        private readonly OvertimeUtility _overtimeUtility;

        public HolidayOvertimeTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var _context = setup.HolidayDbContext;
            _mapper = setup.Mapper;
            var configuration = setup.GetConfiguration();
            _overtimeUtility = new OvertimeUtility(configuration);
            _mockTimeService = new Mock<TimeService>().Object;
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = setup.InitializeUserManager();
            _employeesRepository = new EmployeesRepository(_context, userManager);
            var clientsRepository = new ClientsRepository(_context);
            var mockEmailService = new Mock<IEmailService>();
            var mockDocxGeneratorService = new Mock<IDocxGeneratorService>();
            var mockUserService = new Mock<IUserService>().Object;

            _employeeHolidaysConfirmationUpdater = new EmployeeHolidaysConfirmationUpdater(_employeesRepository, _mockTimeService, _overtimeUtility);
            var holidaysService = new HolidaysService(_holidaysRepository, _employeesRepository, _mapper, _mockTimeService,
                                                      _overtimeUtility, clientsRepository, mockUserService, configuration);
            _holidayConfirmService = new HolidayConfirmService(mockEmailService.Object, _mapper, _holidaysRepository,
                                                               _employeesRepository, clientsRepository, holidaysService,
                                                                mockDocxGeneratorService.Object, _overtimeUtility,
                                                               _employeeHolidaysConfirmationUpdater, new Mock<ILogger<HolidayConfirmService>>().Object);
        }



        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async void When_UpdatingEmployeesOvertime_Expect_UpdatesOvertime(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employeeId = holiday.EmployeeId;
            var employee = await _employeesRepository.GetById(employeeId);
            var initialOvertime = employee.OvertimeHours;
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var expectedOvertime = initialOvertime - _overtimeUtility.ConvertOvertimeDaysToHours(holiday.OvertimeDays);

            await _employeeHolidaysConfirmationUpdater.UpdateEmployeesOvertime(holidayDto);

            employee = await _employeesRepository.GetById(employeeId);
            var actualOvertime = employee.OvertimeHours;

            Assert.Equal(expectedOvertime, actualOvertime);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public async void When_EmployeeUsesOvertime_Expect_ReducesOvertimeOnly(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employeeId = holiday.EmployeeId;
            var employee = await _employeesRepository.GetById(employeeId);

            var expectedVacation = employee.FreeWorkDays;
            var expectedOvertime = employee.OvertimeHours - _overtimeUtility.ConvertOvertimeDaysToHours(holiday.OvertimeDays);

            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = 1
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            employee = await _employeesRepository.GetById(employeeId);

            var vacationAfter = employee.FreeWorkDays;
            var overtimeAfter = employee.OvertimeHours;

            Assert.Equal(expectedVacation, vacationAfter);
            Assert.Equal(expectedOvertime, overtimeAfter);
        }


        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        public async void When_EmployeeUsesOvertimeAndVacation_Expect_ReducesOvertimeAndVacation(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employeeId = holiday.EmployeeId;
            var employee = await _employeesRepository.GetById(employeeId);


            var workDays = _mockTimeService.GetWorkDays(holiday.FromInclusive, holiday.ToInclusive);
            var expectedVacation = employee.FreeWorkDays - workDays + holiday.OvertimeDays;
            var expectedOvertime = employee.OvertimeHours - _overtimeUtility.ConvertOvertimeDaysToHours(holiday.OvertimeDays);

            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = 1
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            employee = await _employeesRepository.GetById(employeeId);

            var vacationAfter = employee.FreeWorkDays;
            var overtimeAfter = employee.OvertimeHours;

            Assert.Equal(expectedVacation, vacationAfter);
            Assert.Equal(expectedOvertime, overtimeAfter);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(9)]
        public async void When_EmployeeUsesVacation_Expect_ReducesVacationOnly(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var employeeId = holiday.EmployeeId;
            var employee = await _employeesRepository.GetById(employeeId);

            var workDays = _mockTimeService.GetWorkDays(holiday.FromInclusive, holiday.ToInclusive);
            var expectedVacation = employee.FreeWorkDays - workDays;
            var expectedOvertime = employee.OvertimeHours;

            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = 1
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            employee = await _employeesRepository.GetById(employeeId);

            var vacationAfter = employee.FreeWorkDays;
            var overtimeAfter = employee.OvertimeHours;

            Assert.Equal(expectedVacation, vacationAfter);
            Assert.Equal(expectedOvertime, overtimeAfter);
        }

    }
}