using System;
using XplicityApp.Infrastructure.Repositories;
using Xunit;
using XplicityApp.Services.BackgroundFunctions;
using System.Linq;
using XplicityApp.Infrastructure.Utils;

namespace Tests.Tests.BackgroundTests
{
    [TestCaseOrderer("Tests.InventoryUpdaterTests.AlphabeticalOrderer", "Tests")]
    public class InventoryUpdaterTests
    {
        const int licenseCategoryId = 4;

        private readonly InventoryItemsRepository _inventoryItemRepository;
        private readonly BackgroundInventoryUpdater _backgroundInventoryUpdater;
        private readonly InventoryCategoryRepository _inventoryCategoryRepository;
        private readonly TimeService _timeService;

        public InventoryUpdaterTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            var context = setup.HolidayDbContext;

            _inventoryItemRepository = new InventoryItemsRepository(context);
            _inventoryCategoryRepository = new InventoryCategoryRepository(context);
            _timeService = new TimeService();

            _backgroundInventoryUpdater = new BackgroundInventoryUpdater(_inventoryItemRepository, _inventoryCategoryRepository, _timeService);
        }

        [Fact]
        public async void When_DepreciatingInventoryItems_Expect_CurrentPriceDepreciated()
        {
            var allInventoryItems = await _inventoryItemRepository.GetAll();
            var formerPrices = allInventoryItems.Where(item => item.InventoryCategoryId != licenseCategoryId)
                                                 .Select(item => item.CurrentPrice).ToList();

            await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            var allUpdatedInventoryItems = await _inventoryItemRepository.GetAll();
            var tangibleItems = allUpdatedInventoryItems.Where(item => item.InventoryCategoryId != licenseCategoryId);

            for (int i = 0; i < tangibleItems.Count(); i++)
            {
                var currentPrice = tangibleItems.ElementAt(i).CurrentPrice;
                var formerPrice = formerPrices.ElementAt(i);
                Assert.True(currentPrice < formerPrice, "Failed to depreciate all tangible items.");
            }
        }

        [Fact]
        public async void When_DepreciatingLicenses_Expect_NoDepreciationApplied()
        {
            var allInventoryItems = await _inventoryItemRepository.GetAll();
            var formerPrices = allInventoryItems.Where(item => item.InventoryCategoryId == licenseCategoryId)
                                                 .Select(item => item.CurrentPrice).ToList();

            await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            var allUpdatedInventoryItems = await _inventoryItemRepository.GetAll();
            var licenses = allUpdatedInventoryItems.Where(item => item.InventoryCategoryId == licenseCategoryId);

            for (int i = 0; i < licenses.Count(); i++)
            {
                var currentPrice = licenses.ElementAt(i).CurrentPrice;
                var formerPrice = formerPrices.ElementAt(i);
                Assert.True(currentPrice == formerPrice, "Item of 'Software license' category was depreciated.");
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void When_FullDepreciationIsApplied_Expect_CurrentPriceEqualsOne(int id)
        {
            var item = await _inventoryItemRepository.GetById(id);
            var category = await _inventoryCategoryRepository.GetById(item.InventoryCategoryId);
            var lifeExpectancyInDays = _timeService.ConvertYearsToDays((int)category.Deprecation);

            for (int i = 0; i < lifeExpectancyInDays; i++)
                await _backgroundInventoryUpdater.ApplyDepreciationToInventoryItems();

            var itemAfterFullDepreciation = await _inventoryItemRepository.GetById(id);
            var priceAfterFullDepreciation = Math.Round(itemAfterFullDepreciation.CurrentPrice);

            Assert.True(priceAfterFullDepreciation == 1, "Failed to apply full depreciation.");
        }
    }
}
