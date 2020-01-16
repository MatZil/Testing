using AutoMapper;
using System;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    [TestCaseOrderer("Tests.InventoryTests.AlphabeticalOrderer", "Tests")]
    public class InventoryTests
    {
        private readonly HolidayDbContext _context;
        private readonly ITestOutputHelper _output;
        private readonly InventoryItemService _inventoryItemService;
        private readonly int _actualNumberOfItemsInInventory;

        public InventoryTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            var contextMapperTuple = setup.Initialize();
            _context = contextMapperTuple.Item1;
            var mapper = contextMapperTuple.Item2;
            _actualNumberOfItemsInInventory = setup.GetCount("inventoryItems");
            var inventoryItemsRepository = new InventoryItemsRepository(_context);
            _inventoryItemService = new InventoryItemService(inventoryItemsRepository,mapper);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_GettingExistingInventoryItemById_Expect_ReturnsInventoryItem(int id)
        {
            var retrievedInventoryItem = await _inventoryItemService.GetById(id);
            Assert.NotNull(retrievedInventoryItem);
        }

        [Theory]
        [InlineData(3)]
        public async void When_GettingNonexistentInventoryItemById_Expect_ReturnsNull(int id)
        {
            var retrievedInventoryItem = await _inventoryItemService.GetById(id);
            _output.WriteLine("Inventory item by this id does not exist");

            Assert.Null(retrievedInventoryItem);
        }

        [Fact]
        public async void When_GettingAllInventoryItems_Expect_ReturnsAllInventoryItems()
        {
            var retrievedInventoryItems = await _inventoryItemService.GetAll();
            var retrievedNumberOfItemsInInventory = retrievedInventoryItems.Count;
            Assert.Equal(_actualNumberOfItemsInInventory, retrievedNumberOfItemsInInventory);
        }

        [Fact]
        public async void When_CreatingInventoryItem_Expect_ReturnsCreatedInventoryItem()
        {
            var newInventoryItem = new NewInventoryItemDto()
            {
                Name = "New inventory item",
                SerialNumber = "new serial number",
                PurchaseDate = DateTime.Today,
                ExpiryDate = null,
                Comment = "new comment",
                Price = 100,
                InventoryCategoryId = 1,
                EmployeeId = 1
            };

            var createdInventoryItem = await _inventoryItemService.Create(newInventoryItem);

            Assert.NotNull(createdInventoryItem);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_UpdatingInventoryItem_Expect_ReturnsUpdatedInventoryItem(int id)
        {
            var initial = _context.InventoryItems.Find(id).Name;

            UpdateInventoryItemDto updatedInventoryItem = new UpdateInventoryItemDto()
            {
                Name = "UpdatedInventoryItemName",
            };
            var updatedInventoryItemName = updatedInventoryItem.Name;

            await _inventoryItemService.Update(id, updatedInventoryItem);
            var inventoryItemNameInDatabase = _context.InventoryItems.Find(id).Name;
            _output.WriteLine(initial + "   >>   " + inventoryItemNameInDatabase);

            Assert.Equal(updatedInventoryItemName, inventoryItemNameInDatabase);
        }
    }
}
