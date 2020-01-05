using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos.Employees;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database.Models;

namespace Xplicity_Holidays.Services.Interfaces
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
