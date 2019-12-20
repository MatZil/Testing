using System;
using System.Collections.Generic;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Utils;
using AutoMapper;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Moq;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xplicity_Holidays.Services.Extractions;

namespace Tests
{
    [TestCaseOrderer("Tests.BackgroundTests.AlphabeticalOrderer", "Tests")]
    public class BackgroundTests
    {
        private readonly HolidayDbContext _context;
        private readonly ITestOutputHelper _output;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly EmployeesRepository _employeesRepository;
        private readonly BackgroundService _backgroundService;
        private readonly TimeService _timeService;
        private readonly BackgroundMethods _accessMethods;

        public BackgroundTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize(out _context, out IMapper mapper);
            _accessMethods = new BackgroundMethods();

            _timeService = new TimeService();
            _holidaysRepository = new HolidaysRepository(_context);
            var userManager = setup.InitializeUserManager(_context);
            _employeesRepository = new EmployeesRepository(_context, userManager);

            var _serviceScopeFactory = new Mock<IServiceScopeFactory>().Object;
            var _hostingEnvironment = new Mock<IHostingEnvironment>().Object;
            _backgroundService = new BackgroundService(_timeService, _serviceScopeFactory, _hostingEnvironment);
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

            await _accessMethods.AddFreeWorkDays(employees, _timeService, _employeesRepository);
            
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
            ICollection<Employee> employees = await _employeesRepository.GetAll();

            var timeService = new Mock<ITimeService>();
            timeService.Setup(m => m.GetCurrentTime()).Returns(new DateTime(2019,01,01));

            var expected = new int[employees.Count, 2];
            var index = 0;
            foreach (var e in employees)
            {
                expected[index, 0] = e.NextMonthAvailableLeaves;
                expected[index++, 1] = e.ParentalLeaveLimit;
            }

            await _accessMethods.ResetParentalLeaves(employees, timeService.Object, _employeesRepository);

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
