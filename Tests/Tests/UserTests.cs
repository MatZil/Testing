using AutoMapper;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;
using XplicityApp.Infrastructure.Database;
using Xunit.Abstractions;
using XplicityApp.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Identity;
using System;
using XplicityApp.Infrastructure.Utils.Interfaces;
using Moq;

namespace Tests
{
    [TestCaseOrderer("Tests.UserTests.AlphabeticalOrderer", "Tests")]
    public class UserTests
    {
        private readonly HolidayDbContext _context;
        private readonly ITestOutputHelper _output;
        private readonly UserService _usersService;
        private readonly UserManager<User> _userManager;
        private readonly IOvertimeUtility _overtimeUtility;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public UserTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;
            _overtimeUtility = new Mock<IOvertimeUtility>().Object;
            _userManager = setup.InitializeUserManager(_context);
            //_roleManager = _setup.InitializeRoleManager(_context);

            EmployeesRepository employeesRepository = new EmployeesRepository(_context, _userManager);
            _usersService = new UserService(_overtimeUtility, _userManager);
        }

        //[Theory]
        //[InlineData(1, "pass", "available@email")]
        //public async void When_CreatingUser_Expect_ReturnsNewUser(int clientId, string password, string email)
        //{
        //    var newEmployeeDto = _setup.NewEmployeeDto(clientId, password, email);
        //    var newEmployee = _setup.NewEmployee(clientId, email);

        //    var createdUser = await _usersService.Create(newEmployee, newEmployeeDto);
        //    Assert.NotNull(createdUser);
        //}

        //[Theory]
        //[InlineData(1)]
        //[InlineData(2)]
        //public async void When_UpdatingUser_Expect_UpdatesUser(int id)
        //{
        //    var initial = _context.Employees.Find(id).Surname;

        //    var updateEmployeeDto = new UpdateEmployeeDto
        //    {
        //        Surname = "Updated Surname"
        //    };
        //    var expected = updateEmployeeDto.Surname;

        //    await _usersService.Update(id, updateEmployeeDto);
        //    var actual = _context.Employees.Find(id).Surname;
        //    _output.WriteLine(initial + "   >>   " + actual);

        //    Assert.Equal(expected, actual);
        //}

        [Theory]
        [InlineData(3)]
        public void When_UpdatingNonexistentUser_Expect_InvalidOperationException(int id)
        {
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Surname = "Updated Surname"
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _usersService.Update(id, updateEmployeeDto));
        }
    }
}
