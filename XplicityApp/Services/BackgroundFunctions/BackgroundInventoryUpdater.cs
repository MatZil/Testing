using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Utils.Interfaces;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class BackgroundInventoryUpdater : IBackgroundInventoryUpdater
    {
        const int licenseCategoryId = 4;

        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IRepository<InventoryCategory> _inventoryCategoryRepository;
        private readonly ITimeService _timeService;

        public BackgroundInventoryUpdater(
            IInventoryItemRepository inventoryItemRepository,
            IRepository<InventoryCategory> inventoryCategoryRepository,
            ITimeService timeService)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryCategoryRepository = inventoryCategoryRepository;
            _timeService = timeService;
        }

        public async Task ApplyDepreciationToInventoryItems()
        {
            var allCurrentlyUsedInventoryItems = await _inventoryItemRepository.GetByStatus(false);
            var inventoryItemsToDepreciate = allCurrentlyUsedInventoryItems.Where(item => item.InventoryCategoryId != licenseCategoryId);

            foreach (var item in inventoryItemsToDepreciate)
            {
                var category = await _inventoryCategoryRepository.GetById(item.InventoryCategoryId);
                var serviceLifeInYears = (int)category.Deprecation;
                var serviceLifeInDays = _timeService.ConvertYearsToDays(serviceLifeInYears);
                var dailyDepreciation = (item.OriginalPrice - 1) / serviceLifeInDays;

                item.CurrentPrice -= (decimal)dailyDepreciation;
            }
        }
    }
}
