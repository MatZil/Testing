using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Repositories
{
    public class InventoryItemTagsRepository : IInventoryItemTagsRepository
    {
        protected readonly HolidayDbContext _context;

        public InventoryItemTagsRepository(HolidayDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(InventoryItemTag entity)
        {
            _context.InventoryItemsTags.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> Delete(InventoryItemTag entity)
        {
            _context.InventoryItemsTags.Remove(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<ICollection<InventoryItemTag>> FindAllByInventoryItemId(int itemId)
        {
            var inventoryItemTags = await _context.InventoryItemsTags.Where(x => x.InventoryItemId == itemId).ToArrayAsync();

            return inventoryItemTags;
        }

        public async Task<InventoryItemTag> FindByInventoryItemIdAndTagId(int itemId, int tagId)
        {
            var inventoryItemTags = await _context.InventoryItemsTags.Where(x => x.InventoryItemId == itemId && x.TagId == tagId).FirstOrDefaultAsync();

            return inventoryItemTags;
        }

        public Task<ICollection<InventoryItemTag>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryItemTag> GetById(int id)
        {
            var inventoryItemTag = await _context.InventoryItemsTags.Where(x => x.Id == id).FirstOrDefaultAsync();

            return inventoryItemTag;
        }

        public async Task<bool> Update(InventoryItemTag entity)
        {
            _context.InventoryItemsTags.Attach(entity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
