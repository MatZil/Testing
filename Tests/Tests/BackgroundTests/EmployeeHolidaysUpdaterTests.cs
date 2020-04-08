using System;
using Moq;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Xunit;
using XplicityApp.Services.BackgroundFunctions;
using Microsoft.Extensions.Logging;
using XplicityApp.Services.Interfaces;

namespace Tests.Tests.BackgroundTests
{
    [TestCaseOrderer("Tests.BackgroundTests.AlphabeticalOrderer", "Tests")]
    public class EmployeeHolidaysUpdaterTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly EmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;

        public EmployeeHolidaysUpdaterTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            var userManager = setup.InitializeUserManager();

            _employeesRepository = new EmployeesRepository(context, userManager);
            _mockTimeService = new Mock<ITimeService>();
            var mockLoggerUpdater = new Mock<ILogger<EmployeeHolidaysBackgroundUpdater>>().Object;
            var mockEmployeesService = new Mock<IEmployeesService>().Object;
            _employeeHolidaysBackgroundUpdater = new EmployeeHolidaysBackgroundUpdater(_mockTimeService.Object, _employeesRepository, mockLoggerUpdater, mockEmployeesService);
        }

        [Fact]
        public async void When_AddingFreeWorkDays_Expect_AddsDaysOff()
        {
            _mockTimeService.Setup(m => m.GetCurrentTime()).Returns(DateTime.MinValue);
            _mockTimeService.Setup(m => m.IsWorkDay(DateTime.MinValue)).Returns(true);

            var employees = await _employeesRepository.GetAll();
            var initial = new double[employees.Count];
            var final = new double[employees.Count];
            var index = 0;
            var countTrue = 0;

            foreach (var employee in employees)
            {
                initial[index++] = employee.FreeWorkDays;
            }

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(employees);
            index = 0;

            foreach (var employee in employees)
            {
                final[index] = employee.FreeWorkDays;

                if (final[index] > initial[index++])
                {
                    countTrue++;
                }
            }

            Assert.Equal(employees.Count, countTrue);
        }

        [Fact]
        public async void When_ResettingParentalLeaves_Expect_ResetsLeaves()
        {
            _mockTimeService.Setup(m => m.GetCurrentTime()).Returns(new DateTime(2019, 01, 01));

            var employees = await _employeesRepository.GetAll();
            var actual = new int[employees.Count, 2];
            var expected = new int[employees.Count, 2];
            var countTrue = 0;
            var index = 0;

            foreach (var employee in employees)
            {
                expected[index, 0] = employee.NextMonthAvailableLeaves;
                expected[index++, 1] = employee.ParentalLeaveLimit;
            }

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(employees);
            index = 0;

            foreach (var employee in employees)
            {
                actual[index, 0] = employee.CurrentAvailableLeaves;
                actual[index, 1] = employee.NextMonthAvailableLeaves;

                if (actual[index, 0] == expected[index, 0] && actual[index, 1] == expected[index++, 1])
                {
                    countTrue++;
                }
            }

            Assert.Equal(employees.Count, countTrue);
        }
    }
}
