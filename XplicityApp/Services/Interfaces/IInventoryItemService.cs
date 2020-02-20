using System.Collections.Generic;
using System.Threading.Tasks;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Dtos.Tags;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IInventoryItemService
    {
        Task<GetInventoryItemDto> GetById(int id);
        Task<ICollection<GetInventoryItemDto>> GetAll();
        Task<ICollection<GetInventoryItemDto>> GetByEmployeeId(int employeeId);
        Task<InventoryItem> Create(NewInventoryItemDto newInventoryItem);
        Task Update(int id, UpdateInventoryItemDto updateInventoryItemDto);
    }
}
