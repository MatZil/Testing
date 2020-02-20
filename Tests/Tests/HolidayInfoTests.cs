using System.Collections.Generic;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.HolidayInfoTests.AlphabeticalOrderer", "Tests")]
    public class HolidayInfoTests
    {
        private readonly HolidayInfoService _holidayInfoService;
        private readonly IRepository<Holiday> _holidayRepository;

        public HolidayInfoTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;
            
            var userManager = setup.InitializeUserManager();

            var employeesRepository = new EmployeesRepository(context, userManager);
            IRepository<Client> clientsRepository = new ClientsRepository(context);
            _holidayRepository = new HolidaysRepository(context);

            _holidayInfoService = new HolidayInfoService(employeesRepository, clientsRepository);
        }

        [Fact]
        public async void When_GettingClientsAndHolidays_Expect_ReturnsListOfClientsWithHolidays()
        {
            ICollection<Holiday> holidays = await _holidayRepository.GetAll();
            var clientsWithHolidays = await _holidayInfoService.GetClientsAndHolidays(holidays);

            Assert.NotNull(clientsWithHolidays);
        }
    }
}
