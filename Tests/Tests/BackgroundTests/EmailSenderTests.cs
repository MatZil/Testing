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
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace Tests.Tests.BackgroundTests
{
    [TestCaseOrderer("Tests.EmailSenderTests.AlphabeticalOrderer", "Tests")]
    public class EmailSenderTests
    {
        private readonly EmployeesRepository _employeesRepository;
        private readonly Mock<ITimeService> _mockTimeService;
        private readonly TimeService _timeService;
        private readonly BackgroundEmailSender _backgroundEmailSender;
        private readonly BackgroundEmailSender _backgroundEmailSenderWithMockedTimeService;
        private readonly Mock<ILogger<BackgroundEmailSender>> _mockLoggerEmailSender;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly HolidaysRepository _holidaysRepository;
        private readonly NotificationSettingsRepository _notificationSettingsRepository;

        public EmailSenderTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            var userManager = setup.InitializeUserManager();

            _employeesRepository = new EmployeesRepository(context, userManager);
            _holidaysRepository = new HolidaysRepository(context);
            _notificationSettingsRepository = new NotificationSettingsRepository(context);
            var mockHolidayInfoService = new Mock<IHolidayInfoService>().Object;
            _mockLoggerEmailSender = new Mock<ILogger<BackgroundEmailSender>>();
            _mockEmailService = new Mock<IEmailService>();

            _timeService = new TimeService();
            _mockTimeService = new Mock<ITimeService>();

            _backgroundEmailSenderWithMockedTimeService = new BackgroundEmailSender(_mockTimeService.Object, _employeesRepository, _holidaysRepository,
                                                                                    _mockEmailService.Object, mockHolidayInfoService, _notificationSettingsRepository,
                                                                                    _mockLoggerEmailSender.Object);
            _backgroundEmailSender = new BackgroundEmailSender(_timeService, _employeesRepository, _holidaysRepository,
                                                               _mockEmailService.Object, mockHolidayInfoService, _notificationSettingsRepository, 
                                                               _mockLoggerEmailSender.Object);
        }

        [Fact]
        public async void When_SendingHolidayReports_Expect_HolidayReportsWereSent()
        {
            var currentDateTime = DateTime.Now;
            var daysInCurrentMonth = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);
            var lastDayOfCurrentMonth = new DateTime(currentDateTime.Year, currentDateTime.Month, daysInCurrentMonth);

            _mockTimeService.Setup(m => m.GetCurrentTime()).Returns(lastDayOfCurrentMonth);

            await _backgroundEmailSenderWithMockedTimeService.SendHolidayReports();

            _mockEmailService.Verify(emailService => emailService.SendThisMonthsHolidayInfo(It.IsAny<ICollection<Employee>>(), It.IsAny<List<(Holiday, Client)>>()));
        }

        [Fact]
        public async void When_BroadcastingCoworkersAbsences_Expect_AbsencesWereBroadcasted()
        {
            await _backgroundEmailSender.BroadcastCoworkersAbsences();

            if (_timeService.IsWorkDay(_timeService.GetCurrentTime()))
            {
                _mockEmailService.Verify(emailService => emailService.NotifyAllAboutUpcomingAbsences(It.IsAny<ICollection<Employee>>(), It.IsAny<ICollection<Holiday>>()));
            }
            else
            {
                _mockEmailService.Verify(emailService => emailService.NotifyAllAboutUpcomingAbsences(It.IsAny<ICollection<Employee>>(), It.IsAny<ICollection<Holiday>>()), Times.Never());
            }
        }

        [Fact]
        public async void When_BroadcastingCoworkersBirthdays_Expect_BirthdaysWereBroadcasted()
        {
            await _backgroundEmailSender.BroadcastCoworkersBirthdays();

            _mockEmailService.Verify(emailService => emailService.SendBirthDayReminder(It.IsAny<ICollection<Employee>>(), It.IsAny<ICollection<Employee>>()));
        }
    }
}
