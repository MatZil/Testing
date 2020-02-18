using Moq;
using System.Collections.Generic;
using System.Linq;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Emailer;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;

namespace Tests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class EmailServiceReceiverTests
    {
        private readonly EmailService _emailService;

        private Employee _employee;
        private ICollection<Employee> _admins;
        private Holiday _holiday;
        private Client _client;

        private List<string> _actualReceiverList;

        public EmailServiceReceiverTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var config = setup.GetConfiguration();
            var emailTemplatesRepository = new EmailTemplatesRepository(setup.HolidayDbContext);

            var mockFileService = new Mock<IFileService>();
            var mockOvertimeUtility = new Mock<IOvertimeUtility>();
            var mockEmailer = new Mock<IEmailer>();
            mockEmailer
                .Setup(emailer => emailer.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((receiver, subject, body) => _actualReceiverList.Add(receiver));

            InitializeEntities();
            _emailService = new EmailService(mockEmailer.Object, emailTemplatesRepository, config, mockFileService.Object, mockOvertimeUtility.Object);
        }

        private void InitializeEntities()
        {
            _employee = EmailServiceSetup.GetInitializedEmployee();
            _admins = EmailServiceSetup.GetInitializedAdmins();
            _holiday = EmailServiceSetup.GetInitializedHoliday();
            _client = EmailServiceSetup.GetInitializedClient();
        }

        [Fact]
        public async void When_ClientConfirms_Expect_CorrectReceiver()
        {
            _actualReceiverList = new List<string>();
            await _emailService.ConfirmHolidayWithClient(_client, _employee, _holiday);
            Assert.Equal(_client.OwnerEmail, _actualReceiverList.FirstOrDefault());
        }

        [Fact]
        public async void When_AdminConfirms_Expect_CorrectReceivers()
        {
            _actualReceiverList = new List<string>();
            var expectedReceivers = new List<string>();
            await _emailService.ConfirmHolidayWithAdmin(_admins, _employee, _holiday, "", "");

            foreach (var admin in _admins)
            {
                expectedReceivers.Add(admin.Email);
            }

            Assert.Equal(expectedReceivers, _actualReceiverList);
        }

        [Fact]
        public async void When_SendMonthlyReport_Expect_CorrectReceivers()
        {
            _actualReceiverList = new List<string>();
            var expectedReceivers = new List<string>();
            var holidays = new List<(Holiday, Client)> { (_holiday, _client) };
            await _emailService.SendThisMonthsHolidayInfo(_admins, holidays);
            foreach (var admin in _admins)
            {
                expectedReceivers.Add(admin.Email);
            }

            Assert.Equal(expectedReceivers, _actualReceiverList);
        }
        
        [Fact]
        public async void When_NotifyingAboutAbsences_Expect_CorrectReceivers()
        {
            _actualReceiverList = new List<string>();
            var expectedReceivers = new List<string>();
            await _emailService.NotifyAllAboutUpcomingAbsences(_admins, new List<Holiday> { _holiday });
            foreach (var admin in _admins)
            {
                expectedReceivers.Add(admin.Email);
            }

            Assert.Equal(expectedReceivers, _actualReceiverList);
        }

        [Fact]
        public async void When_SendingBirthdayReminders_Expect_CorrectReceivers()
        {
            _actualReceiverList = new List<string>();
            var expectedReceivers = new List<string>();
            var employeesWithBirthdays = new List<Employee> { _employee };
            await _emailService.SendBirthDayReminder(employeesWithBirthdays, _admins);
            foreach (var admin in _admins)
            {
                expectedReceivers.Add(admin.Email);
            }

            Assert.Equal(expectedReceivers, _actualReceiverList);
        }

        [Fact]
        public async void When_SendingOrderNotification_Expect_CorrectReceiver()
        {
            _actualReceiverList = new List<string>();
            await _emailService.SendOrderNotification(1, _employee, _admins.FirstOrDefault().Email);
            Assert.Equal(_admins.FirstOrDefault().Email, _actualReceiverList.FirstOrDefault());
        }

        [Fact]
        public async void When_SendingRequestNotification_Expect_CorrectReceiver()
        {
            _actualReceiverList = new List<string>();
            await _emailService.SendRequestNotification(2, _employee.Email);
            Assert.Equal(_employee.Email, _actualReceiverList.FirstOrDefault());
        }
    }
}