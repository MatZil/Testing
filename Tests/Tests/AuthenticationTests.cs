using Microsoft.Extensions.Options;
using Moq;
using XplicityApp.Configurations;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Services;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.AuthenticationTests.AlphabeticalOrderer", "Tests")]
    public class AuthenticationTests
    {
        private readonly ITestOutputHelper _output;
        private readonly AuthenticationService _authService;
        private readonly int _rolesCount;

        public AuthenticationTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize();
            var userManager = setup.UserManager;
            var roleManager = setup.RoleManager;
            _rolesCount = setup.GetCount("roles");

            var mockTimeService = new Mock<TimeService>().Object;
            var opt = Options.Create(new AppSettings());
            _authService = new AuthenticationService(opt, userManager, roleManager, mockTimeService);

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
        public async void When_GettingAllRoles_Expect_ReturnsListOfAllRoles()
        {
            var roles = await _authService.GetAllRoles();

            var expected = _rolesCount;
            var actual = roles.Count;

            Assert.Equal(expected, actual);
        }
    }
}
