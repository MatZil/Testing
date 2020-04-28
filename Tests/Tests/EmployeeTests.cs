using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;
using XplicityApp.Infrastructure.Database;
using Xunit.Abstractions;
using System;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;
using Moq;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.EmployeeTests.AlphabeticalOrderer", "Tests")]
    public class EmployeeTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _employeesCount;
        private readonly ITestOutputHelper _output;
        private readonly EmployeesService _employeesService;
        private readonly ITimeService _mockTimeService;
        private readonly IConfiguration _configuration;

        public EmployeeTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            var mapper = setup.Mapper;
            _employeesCount = setup.GetCount("employees");

            _configuration = setup.GetConfiguration();
            _mockTimeService = new Mock<ITimeService>().Object;
            var overtimeUtility = new OvertimeUtility(_configuration);
            var userManager = setup.InitializeUserManager();
            var mockUserService = new Mock<IUserService>().Object;
            var employeesRepository = new EmployeesRepository(_context, userManager);
            var notificationSettingsRepository = new NotificationSettingsRepository(_context);
            var notificationSettingsService = new NotificationSettingsService(notificationSettingsRepository, mapper);
            _employeesService = new EmployeesService(employeesRepository, mapper, overtimeUtility, _mockTimeService, mockUserService, notificationSettingsService, _configuration);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingEmployeeById_Expect_ReturnsEmployee(int id)
        {
            var retrievedEmployee = await _employeesService.GetById(id);
            _output.WriteLine(retrievedEmployee.Id + "  " + retrievedEmployee.Name + "  " + retrievedEmployee.Surname);

            Assert.NotNull(retrievedEmployee);
        }

        [Theory]
        [InlineData(-1)]
        public async void When_GettingNonexistentEmployeeById_Expect_ReturnsNull(int id)
        {
            var retrievedEmployee = await _employeesService.GetById(id);
            _output.WriteLine("Employee by this id does not exist");

            Assert.Null(retrievedEmployee);
        }

        [Fact]
        public async void When_GettingAllEmployees_Expect_ReturnsAllEmployees()
        {
            var retrievedEmployees = await _employeesService.GetAll();

            _output.WriteLine(retrievedEmployees.Count.ToString());

            foreach (var retrievedEmployee in retrievedEmployees)
            {
                _output.WriteLine(retrievedEmployee.Id + "  " + retrievedEmployee.Name + "  " + retrievedEmployee.Surname);
            }

            Assert.Equal(retrievedEmployees.Count, _employeesCount);
        }

        [Theory]
        [InlineData(EmployeeStatusEnum.Current)]
        [InlineData(EmployeeStatusEnum.Former)]
        public async void When_GettingEmployeesByEmployeeStatus_Expect_ReturnsEmployees(EmployeeStatusEnum employeeStatus)
        {
            var retrievedEmployees = await _employeesService.GetByEmployeeStatus(employeeStatus);
            int actualEmployeesCount = 0;

            var allEmployees = await _employeesService.GetAll();
            foreach (var employee in allEmployees)
            {
                if (employee.Status == employeeStatus)
                {
                    actualEmployeesCount++;
                }
            }
            Assert.Equal(retrievedEmployees.Count, actualEmployeesCount);
        }

        [Theory]
        [InlineData(1, "", "available@email")]
        public void When_CreatingEmployeeWithoutPassword_Expect_PasswordException(int clientId, string password, string email)
        {
            var newEmployee = SetUp.NewEmployeeDto(clientId, password, email);

            var exception = Record.ExceptionAsync(async () => await _employeesService.Create(newEmployee));
            _output.WriteLine(exception.Result.Message);

            Assert.Equal("Password is required", exception.Result.Message);
        }

        [Theory]
        [InlineData(1, "pass", "taken2@email")]
        public void When_CreatingEmployeeWithTakenEmail_Expect_EmailException(int clientId, string password, string email)
        {
            var newEmployee = SetUp.NewEmployeeDto(clientId, password, email);

            var exception = Record.ExceptionAsync(async () => await _employeesService.Create(newEmployee));
            _output.WriteLine(exception.Result.Message);

            Assert.Equal("Email \"" + email + "\" is already taken", exception.Result.Message);
        }

        [Theory]
        [InlineData(2, "taken1@email")]
        public void When_UpdatingEmployeeWithTakenEmail_Expect_EmailException(int id, string email)
        {
            var foundEmployee = _context.Employees.Find(id);

            var updatedEmployee = new UpdateEmployeeDto()
            {
                Email = email,
            };

            var exception = Record.ExceptionAsync(async () => await _employeesService.Update(id, updatedEmployee));
            _output.WriteLine(exception.Result.Message);

            Assert.Equal("Email " + email + " is already taken", exception.Result.Message);
        }
        [Theory]
        [InlineData(2, "email@email")]
        public async void When_UpdatingEmployee_Expect_EmailChanged(int id, string email)
        {
            var updatedEmployee = new UpdateEmployeeDto()
            {
                Email = email,
            };
            await _employeesService.Update(id, updatedEmployee);

            var foundEmployee = _context.Employees.Find(id);

            Assert.Equal(foundEmployee.Email, updatedEmployee.Email);
        }

        [Theory]
        [InlineData(3, "available@email")]
        public void When_UpdatingNonexistentEmployee_Expect_InvalidOperationException(int id, string email)
        {
            var foundEmployee = _context.Employees.Find(id);

            var updatedEmployee = new UpdateEmployeeDto()
            {
                Email = email,
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _employeesService.Update(id, updatedEmployee));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_DeletingEmployee_Expect_True(int id)
        {
            var wasFound = false;
            var wasDeleted = false;

            var found = _context.Employees.Find(id);
            if (found != null)
            {
                wasFound = true;
                _output.WriteLine(found.Id + "  " + found.Name + "  " + found.Surname);
            }

            bool deletedEmployee = await _employeesService.Delete(id);

            found = _context.Employees.Find(id);
            if (found == null && deletedEmployee)
            {
                wasDeleted = true;
                _output.WriteLine("Deleted");
            }

            Assert.True(wasFound && wasDeleted);
        }

        [Theory]
        [InlineData(-1)]
        public async void When_DeletingNonexistentEmployee_Expect_False(int id)
        {
            _output.WriteLine("Couldn't find employee to delete");
            bool deletedEmployee = await _employeesService.Delete(id);

            Assert.False(deletedEmployee);
        }

        [Theory]
        [InlineData(1)]
        public void When_AddOvertimeDays_Expect_DaysAdded(int id)
        {
            var employee = _context.Employees.Find(id);
            employee = _employeesService.AddOvertimeDays(employee);

            Assert.Equal(employee.OvertimeDays, Math.Floor(employee.OvertimeHours / 8));
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("taken1@email", true)]
        [InlineData("notExistingEmail@email.com", false)]
        public async void When_EmailExists_Expect_GetTrueIfEmailExists(string email, bool isExsits)
        {
            Assert.True(await _employeesService.EmailExists(email) == isExsits);
        }

        [Theory]
        [InlineData(1, "pass", "available@email")]
        public async void When_CreatingNewEmployee_Expect_NewEmployeeCreated(int clientId, string password, string email)
        {
            var newEmployee = SetUp.NewEmployeeDto(clientId, password, email);
            newEmployee.IsManualHolidaysInput = true;
            newEmployee.FreeWorkDays = 20;

            var employee = await _employeesService.Create(newEmployee);

            Assert.Equal(employee.FreeWorkDays, newEmployee.FreeWorkDays);
        }

        [Theory]
        [InlineData(1, "pass", "available@email")]
        public async void When_CreatingNewEmployee_Expect_ThrowsArgumentNullException(int clientId, string password, string email)
        {
            var newEmployee = SetUp.NewEmployeeDto(clientId, password, email);
            newEmployee.IsManualHolidaysInput = true;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _employeesService.Create(newEmployee));
        }

        [Fact]
        public async void When_CreatingNullEmployee_Expect_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _employeesService.Create(null));
        }

        [Fact]
        public async void When_GettingConfirmedHolidaysByMonth_Expect_ReturnsHolidaysStartingFromEndOfLastMonth()
        {
            var selectedDate = DateTime.Today;
            var startDate = _mockTimeService.GetCalendarDateFrom(_configuration, selectedDate);
            var selectedMonthBirthdays = await _employeesService.GetBirthdaysByMonth(selectedDate, 1);

            foreach (var holiday in selectedMonthBirthdays)
            {
                Assert.True(holiday.BirthdayDate >= startDate);
            }
        }

        [Fact]
        public async void When_GettingConfirmedHolidaysByMonth_Expect_ReturnsHolidaysLastingToStartOfNextMonth()
        {
            var selectedDate = DateTime.Today;
            var endDate = _mockTimeService.GetCalendarDateTo(_configuration, selectedDate);
            var selectedMonthBirthdays = await _employeesService.GetBirthdaysByMonth(selectedDate, 1);

            foreach (var holiday in selectedMonthBirthdays)
            {
                Assert.True(holiday.BirthdayDate <= endDate);
            }
        }
    }
}
