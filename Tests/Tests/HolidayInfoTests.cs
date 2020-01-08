using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;
using XplicityApp.Infrastructure.Database;
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
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;

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
