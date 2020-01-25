﻿using Moq;
using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Xunit;
using XplicityApp.Services.Extensions;

namespace Tests
{
    [TestCaseOrderer("Tests.BackgroundTests.AlphabeticalOrderer", "Tests")]
    public class BackgroundTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly ITimeService _mockTimeService;
        private readonly EmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;

        public BackgroundTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            _employeeHolidaysBackgroundUpdater = new EmployeeHolidaysBackgroundUpdater();

            _mockTimeService = new Mock<ITimeService>().Object;
            new HolidaysRepository(context);
            var userManager = setup.InitializeUserManager();
            _employeesRepository = new EmployeesRepository(context, userManager);
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

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(_mockTimeService, _employeesRepository);
            
            var countTrue = 0;

            var currentTime = _mockTimeService.GetCurrentTime();
            if (currentTime.DayOfWeek != DayOfWeek.Saturday && currentTime.DayOfWeek != DayOfWeek.Sunday)
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

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(mockTimeService.Object, _employeesRepository);

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

    }
}
