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
using XplicityApp.Infrastructure.Static_Files;

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
        private readonly ClientsRepository _clientsRepository;
        private readonly TimeService _timeService;
        private readonly IOvertimeUtility _mockOvertimeUtility;
        private readonly EmployeeHolidaysConfirmationUpdater _employeeHolidaysConfirmationUpdater;
        private readonly HolidayValidationService _holidayValidationService;
        private readonly HolidaysService _holidaysService;
        //private readonly UserService _userService;

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
            _clientsRepository = new ClientsRepository(_context);
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            var mockEmailService = new Mock<IEmailService>();
            var mockDocxGeneratorService = new Mock<IDocxGeneratorService>();
            _mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            _employeeHolidaysConfirmationUpdater = new EmployeeHolidaysConfirmationUpdater(_employeesRepository, _timeService, _mockOvertimeUtility);

            var mockUserService = new Mock<IUserService>().Object;
            _holidaysService = new HolidaysService(_holidaysRepository, _employeesRepository, _mapper, _timeService, _mockOvertimeUtility, _clientsRepository, mockUserService);
            _holidayConfirmService = new HolidayConfirmService(mockEmailService.Object, _mapper, _holidaysRepository,
                                                               _employeesRepository, clientsRepository, _holidaysService,
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

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_ConfirmingWithClient_Expect_StatusChangedToClientConfirmed(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status.Equals("ClientConfirmed"), "Client failed to confirm holiday.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_ConfirmingHolidayWithAdmin_Expect_StatusChangedToConfirmed(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status.Equals("AdminConfirmed"), "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_ConfirmingHolidayWithAdmin_Expect_ConfirmerFullNameUpdatedToAdminFullName(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var fullNameExpected = await _holidaysService.GetEmployeeFullName(confirmerId);
            var fullNameActual = updatedHoliday.ConfirmerFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Confirmer full name is incorrect.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_ConfirmingHolidayWithClient_Expect_StatusChangedToConfirmedByClient(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            var updatedHoliday = await _holidaysRepository.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status.Equals("ClientConfirmed"), "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_ConfirmingHolidayWithClient_Expect_ConfirmerFullNameUpdatedToClientCompanyName(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = true,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var fullNameExpected = await _holidaysService.GetClientFullName(confirmerId);
            var fullNameActual = updatedHoliday.ConfirmerFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Confirmer full name is incorrect.");
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
        [InlineData(2)]
        public async void When_UpdatingParentalLeaves_Expect_UpdatesParentalLeaves(int holidayId)
        {
            var holiday = await _holidaysRepository.GetById(holidayId);
            var holidayDto = _mapper.Map<GetHolidayDto>(holiday);

            var employee = await _employeesRepository.GetById(holidayDto.EmployeeId);
            var initial = new int[2];
            initial[0] = employee.CurrentAvailableLeaves;
            initial[1] = employee.NextMonthAvailableLeaves;

            await _employeeHolidaysConfirmationUpdater.UpdateDayForChildrenLeaves(holidayDto);

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
                async () => await _holidayValidationService.ValidateHolidayConfirmationReadiness(holidayId, 1));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_DecliningHolidayWithClient_Expect_StatusChangedToRejectedByClient(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            var updatedHoliday = await _holidaysService.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status.Equals("ClientRejected"), "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(3, 1)]
        public async void When_DecliningHolidayWithAdmin_Expect_StatusChangedToRejected(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);

            var updatedHoliday = await _holidaysService.GetById(holidayId);
            var status = updatedHoliday.Status.ToString();

            Assert.True(status.Equals("AdminRejected"), "Failed to confirm holiday.");
        }

        [Theory]
        [InlineData(3, 1)]
        public async void When_DecliningHolidayWithAdmin_Expect_ConfirmerFullNameUpdatedToAdminFullName(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var fullNameExpected = await _holidaysService.GetEmployeeFullName(confirmerId);
            var fullNameActual = updatedHoliday.ConfirmerFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Confirmer's full name is incorrect.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_DecliningHolidayWithClient_Expect_ConfirmerFullNameUpdatedToClientCompanyName(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var fullNameExpected = await _holidaysService.GetClientFullName(confirmerId);
            var fullNameActual = updatedHoliday.ConfirmerFullName;

            Assert.True(fullNameExpected.Equals(fullNameActual), "Confirmer's full name is incorrect.");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_DecliningHolidayWithClient_Expect_DeclineReason(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = false,
                HolidayId = holidayId,
                ConfirmerId = confirmerId,
                RejectionReason = "Holiday was rejected"
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var rejectionReason = updatedHoliday.RejectionReason;

            Assert.True(rejectionReason.Equals("Holiday was rejected"), "Rejection reason was incorrect");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void When_DecliningHolidayWithAdmin_Expect_DeclineReason(int holidayId, int confirmerId)
        {
            UpdateHolidayStatusDto holidayConfimationStatus = new UpdateHolidayStatusDto()
            {
                Confirm = false,
                IsConfirmerAdmin = true,
                HolidayId = holidayId,
                ConfirmerId = confirmerId,
                RejectionReason = "Holiday was rejected"
            };

            await _holidayConfirmService.UpdateHolidayConfirmationStatus(holidayConfimationStatus);
            var updatedHoliday = await _holidaysService.GetById(holidayId);

            var rejectionReason = updatedHoliday.RejectionReason;

            Assert.True(rejectionReason.Equals("Holiday was rejected"), "Rejection reason was incorrect");
        }
    }
}
