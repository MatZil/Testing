﻿using AutoMapper;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Configurations;
using Xunit.Abstractions;
using Microsoft.Extensions.Options;
using System;
using Xplicity_Holidays.Infrastructure.Utils;


namespace Tests
{
    [TestCaseOrderer("Tests.Tests.EmployeeTests.AlphabeticalOrderer", "Tests")]
    public class EmployeeTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _employeesCount;
        private readonly ITestOutputHelper _output;
        private readonly EmployeesService _employeesService;
        private readonly Set_up _setup;

        public EmployeeTests(ITestOutputHelper output)
        {
            _output = output;
            _setup = new Set_up();
            _setup.Initialize(out _context, out IMapper _mapper);
            _employeesCount = _setup.GetCount("employees");

            var timeService = new TimeService();

            var userManager = _setup.InitializeUserManager(_context);
            var userService = new UserService(userManager, _mapper);

            EmployeesRepository employeesRepository = new EmployeesRepository(_context, userManager);
            _employeesService = new EmployeesService(employeesRepository, _mapper, timeService, userService);
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
        [InlineData(3)]
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

        //[Theory]
        //[InlineData(1, "pass", "available@email")]
        //public async void When_CreatingEmployee_Expect_ReturnsNewEmployee(int clientId, string password, string email)
        //{
        //    var newEmployee = _setup.NewEmployeeDto(clientId, password, email);

        //    var createdEmployee = await _employeesService.Create(newEmployee);

        //    Assert.NotNull(createdEmployee);
        //}

        //[Theory]
        //[InlineData(3, "pass", "available@email")]
        //public void When_CreatingEmployeeWithNonexistentClient_Expect_ClientException(int clientId, string password, string email)
        //{
        //    var newEmployee = _setup.NewEmployeeDto(clientId, password, email);

        //    var exception = Record.ExceptionAsync(async () => await _employeesService.Create(newEmployee));
        //    _output.WriteLine(exception.Result.Message);

        //    Assert.Equal("Client not found", exception.Result.Message);
        //}

        [Theory]
        [InlineData(1, "", "available@email")]
        public void When_CreatingEmployeeWithoutPassword_Expect_PasswordException(int clientId, string password, string email)
        {
            var newEmployee = _setup.NewEmployeeDto(clientId, password, email);

            var exception = Record.ExceptionAsync(async () => await _employeesService.Create(newEmployee));
            _output.WriteLine(exception.Result.Message);

            Assert.Equal("Password is required", exception.Result.Message);
        }

        [Theory]
        [InlineData(1, "pass", "taken2@email")]
        public void When_CreatingEmployeeWithTakenEmail_Expect_EmailException(int clientId, string password, string email)
        {
            var newEmployee = _setup.NewEmployeeDto(clientId, password, email);

            var exception = Record.ExceptionAsync(async () => await _employeesService.Create(newEmployee));
            _output.WriteLine(exception.Result.Message);

            Assert.Equal("Email \"" + email + "\" is already taken", exception.Result.Message);
        }

        //[Theory]
        //[InlineData(1, "available@email")]      //email is available
        //[InlineData(1, "taken1@email")]         //email is the same
        //public async void When_UpdatingEmployee_Expect_ReturnsUpdatedEmployee(int id, string email)
        //{
        //    var foundEmployee = _context.Employees.Find(id);
        //    var initial = foundEmployee.Email;

        //    var updatedEmployee = new UpdateEmployeeDto()
        //    {
        //        Email = email,
        //    };

        //    await _employeesService.Update(id, updatedEmployee);
        //    var actual = _context.Employees.Find(id).Email;
        //    _output.WriteLine(initial + "   >>   " + actual);

        //    Assert.Equal(email, actual);
        //}

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
        [InlineData(3)]
        public async void When_DeletingNonexistentEmployee_Expect_False(int id)
        {
            _output.WriteLine("Couldn't find employee to delete");
            bool deletedEmployee = await _employeesService.Delete(id);

            Assert.False(deletedEmployee);
        }

    }
}
