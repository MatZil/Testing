using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;

namespace XplicityApp.Infrastructure.Repositories
{
    public class InventoryItemsRepository : IInventoryItemRepository
    {
        private readonly HolidayDbContext _context;

        public InventoryItemsRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<InventoryItem>> GetByInventoryItemStatus(bool showArchivedInventory)
        {
            var inventoryItems = await _context.InventoryItems.Include(i => i.Category).Include(i => i.Employee).Where(x => x.Archived == showArchivedInventory).ToArrayAsync();
            return inventoryItems;
        }

        public async Task<ICollection<InventoryItem>> GetAll()
        {
            var inventoryItems = await _context.InventoryItems.Include(i => i.Category).Include(i => i.Employee).ToArrayAsync();
            return inventoryItems;
        }

        public async Task<InventoryItem> GetById(int id)
        {
            var inventoryItem = await _context.InventoryItems.Include(i => i.Category).Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return inventoryItem;
        }

        public async Task<int> Create(InventoryItem newItem)
        {
            await _context.InventoryItems.AddAsync(newItem);
            await _context.SaveChangesAsync();
            return newItem.Id;
        }

        public async Task<bool> Update(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Attach(inventoryItem);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(InventoryItem inventoryItem)
        {
            _context.InventoryItems.Remove(inventoryItem);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<ICollection<InventoryItem>> GetByEmployeeId(int employeeId)
        {
            var inventoryItems = _context.InventoryItems.Include(i =>i.Category).Where(i => i.EmployeeId == employeeId);
            return await inventoryItems.ToListAsync();
        }
    }
}
