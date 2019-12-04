using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
using Xunit;
using Xplicity_Holidays.Infrastructure.Database;
using AutoMapper;
using System.Collections.Generic;

namespace Tests
{
    [TestCaseOrderer("Tests.HolidayInfoTests.AlphabeticalOrderer", "Tests")]
    public class HolidayInfoTests
    {
        private readonly HolidayDbContext _context;
        private readonly HolidayInfoService _holidayInfoService;
        private readonly IRepository<Holiday> _holidayRepository;

        public HolidayInfoTests()
        {
            var setup = new SetUp();
            setup.Initialize(out _context, out IMapper mapper);

            var userManager = setup.InitializeUserManager(_context);

            EmployeesRepository employeesRepository = new EmployeesRepository(_context, userManager);
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            _holidayRepository = new HolidaysRepository(_context);

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
