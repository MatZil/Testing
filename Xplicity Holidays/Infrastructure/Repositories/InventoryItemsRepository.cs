using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Infrastructure.Repositories
{
    public class InventoryItemsRepository : IInventoryItemRepository
    {
        private readonly IInventoryItemRepository _repository;

        public InventoryItemsRepository(IInventoryItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<InventoryItem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<InventoryItem> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(InventoryItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(InventoryItem entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(InventoryItem entity)
        {
            throw new NotImplementedException();
        }

    }
}
