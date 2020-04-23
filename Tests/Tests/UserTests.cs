using Microsoft.AspNetCore.Identity;
using System;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.Users;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.UserTests.AlphabeticalOrderer", "Tests")]
    public class UserTests
    {
        private readonly UserService _usersService;
        private readonly HolidayDbContext _context;
        private readonly EmployeesRepository _repository;
        private readonly UserManager<User> _usermanager;

        public UserTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;

            _usermanager = setup.InitializeUserManager();
            _repository = new EmployeesRepository(_context, _usermanager);
            _usersService = new UserService(_usermanager);
        }

        [Theory]
        [InlineData(1)]
        public async void When_UpdatingUserRole_Expect_UpdatesUserRole(int id)
        {
            var updateEmployeeDto = new UpdateEmployeeDto
            {
                Role = "Employee"
            };
            var expected = updateEmployeeDto.Role;

            await _usersService.Update(id, updateEmployeeDto);
            var actual = await _usersService.GetUserRole(id);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        public async void When_GettingUserRole_Expect_ReturnsRole(int id)
        {
            var role = await _usersService.GetUserRole(id);

            Assert.NotNull(role);
        }

        [Theory]
        [InlineData("user1@gmail.com")]
        public async void When_GettingCurrentUser_Expect_ReturnsUser(string email)
        {
            var user = await _usersService.GetCurrentUser(email);

            Assert.NotNull(user);
        }

        [Theory]
        [InlineData(1)]
        public async void When_UpdatingUserEmail_Expect_UpdatesUserEmail(int id)
        {
            var newEmail = "updatedEmail@email.com";

            await _usersService.ChangeEmail(id, newEmail);
            var user = await _usermanager.FindByEmailAsync(newEmail);

            Assert.NotNull(user);
        }

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
