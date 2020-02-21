﻿using Moq;
using System.Collections.Generic;
using System.Linq;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Emailer;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;
using XplicityApp.Infrastructure.Utils;

namespace Tests.Tests.EmailServiceTests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class SubjectTests
    {
        private readonly EmailService _emailService;
        private readonly EmailTemplatesRepository _emailTemplatesRepository;

        private Employee _employee;
        private ICollection<Employee> _admins;
        private Holiday _holiday;
        private Client _client;

        private string _actualSubject;

        public SubjectTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var config = setup.GetConfiguration();
            _emailTemplatesRepository = new EmailTemplatesRepository(setup.HolidayDbContext);

            var mockFileService = new Mock<IFileService>();
            var mockOvertimeUtility = new Mock<IOvertimeUtility>();
            var mockEmailer = new Mock<IEmailer>();
            mockEmailer
                .Setup(emailer => emailer.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((receiver, subject, body) => _actualSubject = subject);

            InitializeEntities();
            _emailService = new EmailService(mockEmailer.Object, _emailTemplatesRepository, config, mockFileService.Object, mockOvertimeUtility.Object);
        }

        private void InitializeEntities()
        {
            _employee = Setup.GetInitializedEmployee();
            _admins = Setup.GetInitializedAdmins();
            _holiday = Setup.GetInitializedHoliday();
            _client = Setup.GetInitializedClient();
        }

        [Fact]
        public async void When_ClientConfirms_Expect_CorrectSubject()
        {
            await _emailService.ConfirmHolidayWithClient(_client, _employee, _holiday);
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.CLIENT_CONFIRMATION)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_AdminConfirms_Expect_CorrectSubject()
        {
            await _emailService.ConfirmHolidayWithAdmin(_admins, _employee, _holiday, "", "");
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.ADMIN_CONFIRMATION)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_SendMonthlyReport_Expect_CorrectSubject()
        {
            var holidays = new List<(Holiday, Client)> { (_holiday, _client) };
            await _emailService.SendThisMonthsHolidayInfo(_admins, holidays);
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.MONTHLY_HOLIDAYS_REPORT)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_NotifyingAboutAbsences_Expect_CorrectSubject()
        {
            await _emailService.NotifyAllAboutUpcomingAbsences(_admins, new List<Holiday> { _holiday });
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.HOLIDAY_REMINDER)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_SendingBirthdayReminders_Expect_CorrectSubject()
        {
            var employeesWithBirthdays = new List<Employee> { _employee };
            await _emailService.SendBirthDayReminder(employeesWithBirthdays, _admins);
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.BIRTHDAY_REMINDER)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_SendingOrderNotification_Expect_CorrectSubject()
        {
            await _emailService.SendOrderNotification(1, _employee, _admins.FirstOrDefault().Email);
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.ORDER_NOTIFICATION)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }

        [Fact]
        public async void When_SendingRequestNotification_Expect_CorrectSubject()
        {
            await _emailService.SendRequestNotification(2, _employee.Email, "ConfirmerName ConfirmerSurname");
            var expectedSubject = (await _emailTemplatesRepository.GetByPurpose(EmailPurposes.REQUEST_NOTIFICATION)).Subject;
            Assert.Equal(expectedSubject, _actualSubject);
        }
    }
}