﻿using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Emailer;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;

namespace Tests.Tests.EmailServiceTests
{
    [TestCaseOrderer("Tests.HolidayTests.AlphabeticalOrderer", "Tests")]
    public class BodyTests
    {
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;

        private Employee _employee;
        private ICollection<Employee> _admins;
        private Holiday _holiday;
        private Client _client;

        private List<string> _actualBodyList;

        public BodyTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _configuration = setup.GetConfiguration();
            var emailTemplatesRepository = new EmailTemplatesRepository(setup.HolidayDbContext);
            var fileRepository = new FileRepository(setup.HolidayDbContext);
            var timeService = new TimeService();
            _fileService = new FileService(fileRepository, _configuration, timeService);

            var mockOvertimeUtility = new Mock<IOvertimeUtility>();
            var mockEmailer = new Mock<IEmailer>();
            mockEmailer
                .Setup(emailer => emailer.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((receiver, subject, body) => _actualBodyList.Add(body));

            InitializeEntities();
            _emailService = new EmailService(mockEmailer.Object, emailTemplatesRepository, _configuration, _fileService, mockOvertimeUtility.Object);
        }

        private string GetPaidString(bool paid)
        {
            return paid ? "Paid" : "Unpaid";
        }

        private void InitializeEntities()
        {
            _employee = Setup.GetInitializedEmployee();
            _admins = Setup.GetInitializedAdmins();
            _holiday = Setup.GetInitializedHoliday();
            _client = Setup.GetInitializedClient();
        }

        [Fact]
        public async void When_ClientConfirms_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            await _emailService.ConfirmHolidayWithClient(_client, _employee, _holiday);
            var expectedBody =
                            $"Hello, {_client.OwnerName},\n\nAn employee {_employee.Name} {_employee.Surname} is intending to go on " +
                            $"{_holiday.Type} holidays from {_holiday.FromInclusive.ToShortDateString()} to {_holiday.ToInclusive.ToShortDateString()} (inclusive).\n\n" +
                            $"Click this link to confirm: {$"{_configuration["AppSettings:RootUrl"]}/api/HolidayClient?holidayId={_holiday.Id}"}\n" +
                            $"Click this link to decline: {_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={_holiday.Id}";

            Assert.Equal(expectedBody, _actualBodyList.FirstOrDefault());
        }

        [Theory]
        [InlineData("This employee has no client that needs to confirm it")]
        [InlineData("This employee's client has already confirmed this holiday")]
        public async void When_AdminConfirms_Expect_CorrectBodies(string clientStatus)
        {
            _actualBodyList = new List<string>();
            var expectedBodies = new List<string>();
            await _emailService.ConfirmHolidayWithAdmin(_admins, _employee, _holiday, clientStatus, "");

            foreach (var admin in _admins)
            {
                var expectedBody =
                    $"Hello, {admin.Name},\n\nAn employee {_employee.Name} {_employee.Surname} is intending to go on {GetPaidString(_holiday.Paid)} " +
                    $"{_holiday.Type} holidays from {_holiday.FromInclusive.ToShortDateString()} to {_holiday.ToInclusive.ToShortDateString()} (inclusive). " +
                    $"{clientStatus} \n\nClick this link to confirm: {$"{_configuration["AppSettings:RootUrl"]}/api/HolidayConfirm/{_holiday.Id}"}\n" +
                    $"Click this link to decline: {$"{_configuration["AppSettings:RootUrl"]}/api/HolidayDecline?holidayId={_holiday.Id}"}";

                expectedBodies.Add(expectedBody);
            }

            Assert.Equal(expectedBodies, _actualBodyList);
        }

        [Fact]
        public async void When_SendMonthlyReport_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            var holidays = new List<(Holiday, Client)> { (_holiday, _client) };
            await _emailService.SendThisMonthsHolidayInfo(_admins, holidays);
            var expectedBody =
                $"{_client.CompanyName} team's employees:\n\n\n{_employee.Name} {_employee.Surname} went on {GetPaidString(_holiday.Paid)} " +
                $"{_holiday.Type} holidays from {_holiday.FromInclusive.ToShortDateString()} to {_holiday.ToInclusive.ToShortDateString()} (inclusive). \n\n";


            Assert.Equal(expectedBody, _actualBodyList.FirstOrDefault());
        }

        [Fact]
        public async void When_NotifyingAboutAbsences_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            await _emailService.NotifyAllAboutUpcomingAbsences(_admins, new List<Holiday> { _holiday });
            var expectedBody = new StringBuilder();
            expectedBody.AppendLine(
                $"Your colleague {_holiday.Employee.Name} {_holiday.Employee.Surname} is going away for holidays next workday from " +
                $"{_holiday.FromInclusive.ToShortDateString()} to {_holiday.ToInclusive.ToShortDateString()} (inclusive).");


            Assert.Equal(expectedBody.ToString(), _actualBodyList.FirstOrDefault());
        }

        [Fact]
        public async void When_SendingBirthdayReminders_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            var employeesWithBirthdays = new List<Employee> { _employee };
            await _emailService.SendBirthDayReminder(employeesWithBirthdays, _admins);
            var expectedBody = $"Your colleague {_employee.Name} {_employee.Surname} is having their birthday today! Don't forget to congratulate them.";
            Assert.Equal(expectedBody, _actualBodyList.FirstOrDefault());
        }

        [Fact]
        public async void When_SendingOrderNotification_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            await _emailService.SendOrderNotification(1, _employee, _admins.FirstOrDefault().Email);
            var expectedBody =
                $"A holiday order for {_employee.Name} {_employee.Surname} has been generated. " +
                $"Click this link to download it: {_fileService.GetDownloadLink(1)}";
            Assert.Equal(expectedBody, _actualBodyList.FirstOrDefault());
        }

        [Fact]
        public async void When_SendingRequestNotification_Expect_CorrectBody()
        {
            _actualBodyList = new List<string>();
            await _emailService.SendRequestNotification(2, _employee.Email);
            var expectedBody = $"You can download your holiday request document by clicking this link: {_fileService.GetDownloadLink(2)}";
            Assert.Equal(expectedBody, _actualBodyList.FirstOrDefault());
        }
    }
}