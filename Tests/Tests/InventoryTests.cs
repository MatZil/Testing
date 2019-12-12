using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services;
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
        private readonly int _inventoryItemsCount;

        public InventoryTests(ITestOutputHelper output)
        {
            _output = output;
            var setup = new SetUp();
            setup.Initialize(out _context, out IMapper mapper);
            _inventoryItemsCount = setup.GetCount("inventoryItems");
            InventoryItemsRepository inventoryItemsRepository = new InventoryItemsRepository(_context);
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

            Assert.Equal(_inventoryItemsCount, retrievedInventoryItems.Count);
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
            var expected = updatedInventoryItem.Name;

            await _inventoryItemService.Update(id, updatedInventoryItem);
            var actual = _context.InventoryItems.Find(id).Name;
            _output.WriteLine(initial + "   >>   " + actual);

            Assert.Equal(expected, actual);
        }
    }
}
