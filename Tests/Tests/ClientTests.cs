using Xunit;
using XplicityApp.Services;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using AutoMapper;
using XplicityApp.Dtos.Clients;
using Xunit.Abstractions;
using System;


namespace Tests
{
    [TestCaseOrderer("Tests.ClientTests.AlphabeticalOrderer", "Tests")]
    public class ClientTests
    {
        private readonly HolidayDbContext _context;
        private readonly int _clientsCount;
        private readonly ITestOutputHelper _output;
        private readonly ClientsService _clientsService;

        public ClientTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;
            _clientsCount = setup.GetCount("clients");
            IRepository<Client> clientsRepository = new ClientsRepository(_context);
            _clientsService = new ClientsService(clientsRepository, mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingExistingClientById_Expect_ReturnsClient(int id)
        {
            var retrievedClient = await _clientsService.GetById(id);

            Assert.NotNull(retrievedClient);
        }

        [Theory]
        [InlineData(3)]
        public async void When_GettingNonexistentClientById_Expect_ReturnsNull(int id)
        {
            var retrievedClient = await _clientsService.GetById(id);
            _output.WriteLine("Client by this id does not exist");

            Assert.Null(retrievedClient);
        }

        [Fact]
        public async void When_GettingAllClients_Expect_ReturnsAllClients()
        {
            var retrievedClients = await _clientsService.GetAll();

            Assert.Equal(_clientsCount, retrievedClients.Count);
        }

        [Fact]
        public async void When_CreatingClient_Expect_ReturnsCreatedClient()
        {
            var newClient = new NewClientDto()
            {
                CompanyName = "NewCompanyName",
                OwnerName = "NewName",
                OwnerSurname = "NewSurname",
                OwnerEmail = "new@gmail.com",
                OwnerPhone = "111"
            };

            var createdClient = await _clientsService.Create(newClient);

            Assert.NotNull(createdClient);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_DeletingClient_Expect_True(int id)
        {
            var wasFound = false;
            var wasDeleted = false;

            var found = _context.Clients.Find(id);
            if (found != null)
            {
                wasFound = true;
                _output.WriteLine(found.Id + "  " + found.CompanyName);
            }

            bool deletedClient = await _clientsService.Delete(id);

            found = _context.Clients.Find(id);
            if (found == null && deletedClient)
            {
                wasDeleted = true;
                _output.WriteLine("Deleted");
            }

            Assert.True(wasFound && wasDeleted);
        }

        [Theory]
        [InlineData(3)]
        public async void When_DeletingNonexistentClient_Expect_False(int id)
        {
            _output.WriteLine("Couldn't find client to delete");

            bool deletedClient = await _clientsService.Delete(id);

            Assert.False(deletedClient);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingClient_Expect_ReturnsUpdatedClient(int id)
        {
            var initial = _context.Clients.Find(id).CompanyName;

            NewClientDto updatedClient = new NewClientDto()
            {
                CompanyName = "UpdatedCompanyName",
            };
            var expected = updatedClient.CompanyName;

            await _clientsService.Update(id, updatedClient);
            var actual = _context.Clients.Find(id).CompanyName;
            _output.WriteLine(initial + "   >>   " + actual);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(3)]
        public void When_UpdatingNonexistentEmployee_Expect_InvalidOperationException(int id)
        {
            NewClientDto updatedClient = new NewClientDto()
            {
                CompanyName = "UpdatedUAB",
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () => await _clientsService.Update(id, updatedClient));
        }
    }
}
