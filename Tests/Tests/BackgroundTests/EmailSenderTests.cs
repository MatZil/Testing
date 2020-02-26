using Moq;
using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using Xunit;
using XplicityApp.Services.BackgroundFunctions;
using Microsoft.Extensions.Logging;
using XplicityApp.Services.Interfaces;
using XplicityApp.Infrastructure.Utils;

namespace Tests.Tests.BackgroundTests
{
    [TestCaseOrderer("Tests.EmailSenderTests.AlphabeticalOrderer", "Tests")]
    public class EmailSenderTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly TimeService _timeService;
        private readonly BackgroundEmailSender _backgroundEmailSender;
        private readonly Mock<ILogger<BackgroundEmailSender>> _mockLoggerEmailSender;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly HolidaysRepository _holidaysRepository;

        public EmailSenderTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            var userManager = setup.InitializeUserManager();

            _employeesRepository = new EmployeesRepository(context, userManager);
            _timeService = new TimeService();
            _holidaysRepository = new HolidaysRepository(context);
            var holidayInfoService = new Mock<IHolidayInfoService>().Object;
            _mockLoggerEmailSender = new Mock<ILogger<BackgroundEmailSender>>();
            _mockEmailService = new Mock<IEmailService>();

            _backgroundEmailSender = new BackgroundEmailSender(_timeService, _employeesRepository, _holidaysRepository,
                                                               _mockEmailService.Object, holidayInfoService, _mockLoggerEmailSender.Object);
        }

        [Fact]
        public async void When_SendingHolidayReports_Expect_HolidayReportsWereSent()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "TestSendingHolidayReports");

            await _backgroundEmailSender.SendHolidayReports();

            _mockEmailService.Verify(emailService => emailService.SendThisMonthsHolidayInfo(It.IsAny<ICollection<Employee>>(), It.IsAny<List<(Holiday, Client)>>()));
        }

        [Fact]
        public async void When_BroadcastingCoworkersAbsences_Expect_AbsencesWereBroadcasted()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");

            await _backgroundEmailSender.BroadcastCoworkersAbsences();

            _mockEmailService.Verify(emailService => emailService.NotifyAllAboutUpcomingAbsences(It.IsAny<ICollection<Employee>>(), It.IsAny<ICollection<Holiday>>()));
        }

        [Fact]
        public async void When_BroadcastingCoworkersBirthdays_Expect_BirthdaysWereBroadcasted()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");

            await _backgroundEmailSender.BroadcastCoworkersBirthdays();

            _mockEmailService.Verify(emailService => emailService.SendBirthDayReminder(It.IsAny<ICollection<Employee>>(), It.IsAny<ICollection<Employee>>()));
        }
    }
}
