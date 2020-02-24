using Moq;
using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Xunit;
using Nager.Date;
using XplicityApp.Services.BackgroundFunctions;
using Microsoft.Extensions.Logging;
using XplicityApp.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.BackgroundTests.AlphabeticalOrderer", "Tests")]
    public class BackgroundTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly ITimeService _mockTimeService;
        private readonly EmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;
        private readonly BackgroundEmailSender _backgroundEmailSender;
        private readonly Mock<ILoggerAdapter<BackgroundEmailSender>> _mockLoggerEmailSender;

        public BackgroundTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            var userManager = setup.InitializeUserManager();

            _employeesRepository = new EmployeesRepository(context, userManager);
            _mockTimeService = new Mock<ITimeService>().Object;
            var mockLoggerUpdater = new Mock<ILogger<EmployeeHolidaysBackgroundUpdater>>().Object;
            var mockEmployeesService = new Mock<IEmployeesService>().Object;
            _employeeHolidaysBackgroundUpdater = new EmployeeHolidaysBackgroundUpdater(_mockTimeService, _employeesRepository, mockLoggerUpdater, mockEmployeesService);

            var holidaysRepository = new HolidaysRepository(context);
            var mockEmailService = new Mock<IEmailService>().Object;
            var holidayInfoService = new Mock<IHolidayInfoService>().Object;
            var webHostEnvironment = new Mock<IWebHostEnvironment>().Object;
            _mockLoggerEmailSender = new Mock<ILoggerAdapter<BackgroundEmailSender>>();

            _backgroundEmailSender = new BackgroundEmailSender(_mockTimeService, _employeesRepository, holidaysRepository, 
                                                                mockEmailService, holidayInfoService, webHostEnvironment, _mockLoggerEmailSender.Object);
        }

        [Fact]
        public async void When_AddingFreeWorkDays_Expect_AddsDaysOff()
        {
            ICollection<Employee> employees = await _employeesRepository.GetAll();

            var initial = new double[employees.Count];
            var index = 0;
            foreach (var e in employees)
            {
                initial[index++] = e.FreeWorkDays;
            }

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(employees);
            var countTrue = 0;

            var currentTime = _mockTimeService.GetCurrentTime();
            if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday && !DateSystem.IsPublicHoliday(currentTime, CountryCode.LT))
            {
                var final = new double[employees.Count];
                index = 0;
                foreach (var e in employees)
                {
                    final[index] = e.FreeWorkDays;

                    if (final[index] > initial[index++])
                    {
                        countTrue++;
                    }
                }
            }
            else
            {
                countTrue = employees.Count;
            }

            Assert.Equal(employees.Count, countTrue);
        }

        [Fact]
        public async void When_ResettingParentalLeaves_Expect_ResetsLeaves()
        {
            var employees = await _employeesRepository.GetAll();

            var mockTimeService = new Mock<ITimeService>();
            mockTimeService.Setup(m => m.GetCurrentTime()).Returns(new DateTime(2019,01,01));

            var expected = new int[employees.Count, 2];
            var index = 0;
            foreach (var e in employees)
            {
                expected[index, 0] = e.NextMonthAvailableLeaves;
                expected[index++, 1] = e.ParentalLeaveLimit;
            }

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(employees);

            var countTrue = 0;
            var actual = new int[employees.Count, 2];
            index = 0;
            foreach (var e in employees)
            {
                actual[index, 0] = e.CurrentAvailableLeaves;
                actual[index, 1] = e.NextMonthAvailableLeaves;

                if (actual[index, 0] == expected[index, 0] &&
                    actual[index, 1] == expected[index++, 1])
                    countTrue++;
            }

            Assert.Equal(employees.Count, countTrue);
        }

        [Fact]
        public async void When_SendingHolidayReports_Expect_HolidayReportsWereSent()
        {
            await _backgroundEmailSender.SendHolidayReports();

            var currentTime = _mockTimeService.GetCurrentTime();
            _mockLoggerEmailSender.Verify(l => l.LogInformation("SendHolidayReports() ended at "+ currentTime));
        }

        [Fact]
        public async void When_BroadcastingCoworkersAbsences_Expect_AbsencesWereBroadcasted()
        {
            await _backgroundEmailSender.BroadcastCoworkersAbsences();

            var currentTime = _mockTimeService.GetCurrentTime();
            _mockLoggerEmailSender.Verify(l => l.LogInformation("BroadcastCoworkersAbsences() ended at " + currentTime));
        }

        [Fact]
        public async void When_BroadcastingCoworkersBirthdays_Expect_BirthdaysWereBroadcasted()
        {
            await _backgroundEmailSender.BroadcastCoworkersBirthdays();

            var currentTime = _mockTimeService.GetCurrentTime();
            _mockLoggerEmailSender.Verify(l => l.LogInformation("BroadcastCoworkersBirthdays() ended at " + currentTime));
        }
    }
}
