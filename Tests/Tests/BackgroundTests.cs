using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using Xunit;
using XplicityApp.Services.Extensions;

namespace Tests
{
    [TestCaseOrderer("Tests.BackgroundTests.AlphabeticalOrderer", "Tests")]
    public class BackgroundTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly BackgroundService _backgroundService;
        private readonly TimeService _timeService;
        private readonly EmployeeHolidaysBackgroundUpdater _employeeHolidaysBackgroundUpdater;

        public BackgroundTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            _employeeHolidaysBackgroundUpdater = new EmployeeHolidaysBackgroundUpdater();

            _timeService = new TimeService();
            new HolidaysRepository(context);
            var userManager = setup.InitializeUserManager();
            _employeesRepository = new EmployeesRepository(context, userManager);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>().Object;
            var hostingEnvironment = new Mock<IWebHostEnvironment>().Object;
            _backgroundService = new BackgroundService(_timeService, serviceScopeFactory, hostingEnvironment,
                                                       _employeeHolidaysBackgroundUpdater);
        }

        //[Fact]
        //public async void When_CheckingForLastMonthDay_Expect_True()
        //{
        //    var admin = new Employee
        //    {
        //        ClientId = 1,
        //        Name = "adminName",
        //        Surname = "adminSurname",
        //        Email = "adminEmail@email"
        //    };

        //    ICollection<Holiday> holidays = await _holidaysRepository.GetAll();
        //    var emailService = new Mock<IEmailService>().Object;
        //    IRepository<Client> clientsRepository = new ClientsRepository(_context);
        //    var holidayInfoService = new HolidayInfoService(_employeesRepository, clientsRepository);

        //    Object[] args = { admin, holidays, emailService, holidayInfoService };
        //    _backgroundService.call("CheckForLastMonthDay", args);
        //}

            
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

            await _employeeHolidaysBackgroundUpdater.AddFreeWorkDays(employees, _timeService, _employeesRepository);
            
            var countTrue = 0;

            var currentTime = _timeService.GetCurrentTime();
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

            var timeService = new Mock<ITimeService>();
            timeService.Setup(m => m.GetCurrentTime()).Returns(new DateTime(2019,01,01));

            var expected = new int[employees.Count, 2];
            var index = 0;
            foreach (var e in employees)
            {
                expected[index, 0] = e.NextMonthAvailableLeaves;
                expected[index++, 1] = e.ParentalLeaveLimit;
            }

            await _employeeHolidaysBackgroundUpdater.ResetParentalLeaves(employees, timeService.Object, _employeesRepository);

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
