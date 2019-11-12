using AutoMapper;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using Xunit.Abstractions;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Microsoft.AspNetCore.Identity;
using Xplicity_Holidays.Configurations;
using Microsoft.Extensions.Options;

namespace Tests
{
    [TestCaseOrderer("Tests.Tests.AuthenticationTests.AlphabeticalOrderer", "Tests")]
    public class AuthenticationTests
    {
        private readonly HolidayDbContext _context;
        private readonly ITestOutputHelper _output;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationService _authService;
        private readonly int _rolesCount;
        private readonly Set_up _setup;

        public AuthenticationTests(ITestOutputHelper output)
        {
            _output = output;
            _setup = new Set_up();
            _setup.Initialize(out _context, out IMapper mapper);
            _rolesCount = _setup.GetCount("roles");

            _userManager = _setup.InitializeUserManager(_context);
            _roleManager = _setup.InitializeRoleManager(_context);

            var opt = Options.Create<AppSettings>(new AppSettings());
            _authService = new AuthenticationService(opt, _userManager, _roleManager);
        }

        //[Theory]
        //[InlineData("email", "password")]
        //public async void When_Authenticating_Expect_ReturnsAuthenticatedUser(string email, string password)
        //{
        //    var authenticatedUser = await _authService.Authenticate(email, password);

        //    //var user = await _userManager.FindByEmailAsync(email);
        //    //_output.WriteLine(user.Email);

        //    Assert.NotNull(authenticatedUser);
        //}

        //[Fact]
        //public async void When_CreatingJwt_Expect_CreatesToken()
        //{
        //    var user = _setup.NewUser();

        //    _authService.call("CreateJwt", user);

        //    //await _userManager.CreateAsync(user);
        //    //await _userManager.AddToRoleAsync(user, "Admin");
        //    //var roles = await _userManager.GetRolesAsync(user);

        //    // _output.WriteLine(roles.Count.ToString());

        //    //Assert.NotNull(roles);
        //}

        [Fact]
        public async void When_GettingAllRoles_Expect_ReturnsListOfRoles()
        {
            var roles = await _authService.GetAllRoles();

            foreach (var role in roles)
                _output.WriteLine(role.Name);

            var expected = _rolesCount;
            var actual = roles.Count;

            Assert.Equal(expected, actual);
        }
    }
}
