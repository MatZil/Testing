using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.BackgroundFunctions.Interfaces;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Utils.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace XplicityApp.Services.BackgroundFunctions
{
    public class BackgroundInventoryUpdater : IBackgroundInventoryUpdater
    {
        const int licenseCategoryId = 4;

        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IRepository<InventoryCategory> _inventoryCategoryRepository;
        private readonly ITimeService _timeService;
        private readonly ILogger<BackgroundInventoryUpdater> _logger;

        public BackgroundInventoryUpdater(
            IInventoryItemRepository inventoryItemRepository,
            IRepository<InventoryCategory> inventoryCategoryRepository,
            ITimeService timeService,
            ILogger<BackgroundInventoryUpdater> logger)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryCategoryRepository = inventoryCategoryRepository;
            _timeService = timeService;
            _logger = logger;
        }

        public async Task ApplyDepreciationToInventoryItems()
        {
            var currentTime = _timeService.GetCurrentTime();
            _logger.LogInformation("ApplyDepreciationToInventoryItems() was initiated at " + currentTime);

            var allCurrentlyUsedInventoryItems = await _inventoryItemRepository.GetByStatus(false);
            var inventoryItemsToDepreciate = allCurrentlyUsedInventoryItems.Where(item => item.InventoryCategoryId != licenseCategoryId);
            
            try
            {
                foreach (var item in inventoryItemsToDepreciate)
                {
                    var category = await _inventoryCategoryRepository.GetById(item.InventoryCategoryId);
                    var serviceLifeInYears = (int)category.Deprecation;
                    var serviceLifeInDays = _timeService.ConvertYearsToDays(serviceLifeInYears);
                    var dailyDepreciation = (item.OriginalPrice - 1) / serviceLifeInDays;

                    item.CurrentPrice -= (decimal)dailyDepreciation;

                    await _inventoryItemRepository.Update(item);
                }
            }
            catch (Exception exception)
            {
                _logger.LogInformation(exception.ToString() + " occurred in ApplyDepreciationToInventoryItems() at " + currentTime);
            }

        _logger.LogInformation("ApplyDepreciationToInventoryItems() ended at " + currentTime);
        }
    }
}
