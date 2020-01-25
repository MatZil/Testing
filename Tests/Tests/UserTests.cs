using System;
using XplicityApp.Dtos.Employees;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests
{
    [TestCaseOrderer("Tests.UserTests.AlphabeticalOrderer", "Tests")]
    public class UserTests
    {
        private readonly UserService _usersService;
        
        public UserTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;

            var userManager = setup.InitializeUserManager();
            
            new EmployeesRepository(context, userManager);
            _usersService = new UserService(userManager);
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
