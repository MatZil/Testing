using System;
using System.Collections.Generic;
using XplicityApp.Infrastructure.Repositories;
using Xunit;
using XplicityApp.Services.BackgroundFunctions;
using Xunit.Abstractions;

namespace Tests.Tests.BackgroundTests
{
    [TestCaseOrderer("Tests.InventoryUpdaterTests.AlphabeticalOrderer", "Tests")]
    public class InventoryUpdaterTests
    {
        private readonly InventoryItemsRepository _inventoryItemRepository;
        private readonly BackgroundInventoryUpdater _backgroundInventoryUpdater;
        private readonly InventoryCategoryRepository _inventoryCategoryRepository;

        public InventoryUpdaterTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;

            _inventoryItemRepository = new InventoryItemsRepository(context);
            _inventoryCategoryRepository = new InventoryCategoryRepository(context);

            _backgroundInventoryUpdater = new BackgroundInventoryUpdater(_inventoryItemRepository, _inventoryCategoryRepository);
        }

        [Fact]
        public async void When_DepreciatingInventoryItems_Expect_CurrentPriceDepreciated()
        {
            var allInventoryItems = await _inventoryItemRepository.GetAll();
            var currentPrices = new List<decimal>();

            foreach (var item in allInventoryItems)
            {
                currentPrices.Add(item.CurrentPrice);
            }

            await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            var allUpdatedInventoryItems = await _inventoryItemRepository.GetAll();

            foreach (var item in allUpdatedInventoryItems)
            {
                if (item.InventoryCategoryId != 4)
                {
                    Assert.True(item.CurrentPrice < currentPrices[item.Id - 1], "Failed to depreciate item.");
                }
                else if (item.InventoryCategoryId == 4)
                {
                    Assert.True(item.CurrentPrice == currentPrices[item.Id - 1], "Item of 'Software license' category was depreciated.");
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_FullDepreciationIsApplied_Expect_CurrentPriceEqualsOne(int id)
        {
            var item = await _inventoryItemRepository.GetById(id);
            var category = await _inventoryCategoryRepository.GetById(item.InventoryCategoryId);

            for (int i = 0; i < category.Deprecation * 365; i++)
                await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            var itemAfterFullDepreciation = await _inventoryItemRepository.GetById(id);
            var roundedValue = Math.Round(itemAfterFullDepreciation.CurrentPrice);

            Assert.True(roundedValue == 1);
        }
    }
}
