using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class BackgroundInventoryUpdater : IBackgroundInventoryUpdater
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IRepository<InventoryCategory> _inventoryCategoryRepository;

        public BackgroundInventoryUpdater(
            IInventoryItemRepository inventoryItemRepository,
            IRepository<InventoryCategory> inventoryCategoryRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryCategoryRepository = inventoryCategoryRepository;
        }

        public async Task ApplyDepreciationToInventoryItems()
        {
            var allCurrentlyUsedInventoryItems = await _inventoryItemRepository.GetByStatus(false);
            var inventoryItemsToDepreciate = allCurrentlyUsedInventoryItems.Where(item => item.InventoryCategoryId != 4).ToList();

            foreach (var item in inventoryItemsToDepreciate)
            {
                var category = await _inventoryCategoryRepository.GetById(item.InventoryCategoryId);
                var serviceLifeInDays = category.Deprecation * 365;
                var dailyDepreciation = (item.OriginalPrice - 1) / serviceLifeInDays;

                item.CurrentPrice -= (decimal)dailyDepreciation;
            }
        }
    }
}
