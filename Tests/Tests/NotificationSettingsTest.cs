using Moq;
using System.Threading.Tasks;
using XplicityApp.Dtos.Employees;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Infrastructure.Utils;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services;
using XplicityApp.Services.Interfaces;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.NotificationSettingsTest.AlphabeticalOrderer", "Tests")]
    public class NotificationSettingsTest
    {
        private readonly HolidayDbContext _context;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly EmployeesService _employeesService;
        private readonly int _notificationSettingsCount;

        public NotificationSettingsTest()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            var mapper = setup.Mapper;
            _notificationSettingsCount = setup.GetCount("notificationSettings");

            var notificationSettingsRepository = new NotificationSettingsRepository(_context);
            _notificationSettingsService = new NotificationSettingsService(notificationSettingsRepository, mapper);

            var mockEmployeeRepository = new Mock<IEmployeeRepository>().Object;
            var mockOvertimeUtility = new Mock<IOvertimeUtility>().Object;
            var mockTimeService = new Mock<TimeService>().Object;
            var mockUserService = new Mock<IUserService>().Object;

            _employeesService = new EmployeesService(mockEmployeeRepository, mapper, mockOvertimeUtility, mockTimeService, mockUserService, _notificationSettingsService);
        }
        [Theory]
        [InlineData(1, "password", "available@email")]
        public async Task When_CreateEmployeeAndNotificationSettings_Expect_NotificationSettingsObjectCreated(int clientId, string password, string email)
        {
            var newEmployee = new NewEmployeeDto
            {
                ClientId = clientId,
                Password = password,
                Email = email
            };

            await _employeesService.Create(newEmployee);

            Assert.Equal(_notificationSettingsCount + 1, (await _notificationSettingsService.GetAll()).Length);
        }

        [Theory]
        [InlineData(1, false)]
        public async Task When_UpdateBroadcastOwnBirthdaySetting_Expect_NotificationSettingIsChanged(int id, bool settingValue)
        {
            var notificationSettingsDto = await _notificationSettingsService.GetByEmployeeId(id);
            notificationSettingsDto.BroadcastOwnBirthday = settingValue;

            await _notificationSettingsService.Update(id, notificationSettingsDto);

            var expectedNotificationSettingsDto = await _notificationSettingsService.GetByEmployeeId(id);
            Assert.Equal(settingValue, expectedNotificationSettingsDto.BroadcastOwnBirthday);
        }

        [Theory]
        [InlineData(1, false)]
        public async Task When_UpdateReceiveBirthdayNotificationSetting_Expect_NotificationSettingIsChanged(int id, bool settingValue)
        {
            var notificationSettingsDto = await _notificationSettingsService.GetByEmployeeId(id);
            notificationSettingsDto.ReceiveBirthdayNotifications = settingValue;

            await _notificationSettingsService.Update(id, notificationSettingsDto);

            var expectedNotificationSettingsDto = await _notificationSettingsService.GetByEmployeeId(id);
            Assert.Equal(settingValue, expectedNotificationSettingsDto.ReceiveBirthdayNotifications);
        }
    }
}
