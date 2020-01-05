using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _repository;
        private readonly IMapper _mapper;

        public InventoryItemService(IInventoryItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetInventoryItemDto> GetById(int id)
        {
            var inventoryItem = await _repository.GetById(id);
            var inventoryItemDto = _mapper.Map<GetInventoryItemDto>(inventoryItem);
            return inventoryItemDto;
        }

        public async Task<ICollection<GetInventoryItemDto>> GetAll()
        {
            var inventoryItems = await _repository.GetAll();
            var inventoryItemsDto = _mapper.Map<GetInventoryItemDto[]>(inventoryItems);
            return inventoryItemsDto;
        }

        public async Task<ICollection<GetInventoryItemDto>> GetByEmployeeId(int employeeId)
        {
            var inventoryItems = await _repository.GetByEmployeeId(employeeId);
            var inventoryItemsDto = _mapper.Map<GetInventoryItemDto[]>(inventoryItems);
            return inventoryItemsDto;
        }

        public async Task<InventoryItem> Create(NewInventoryItemDto newInventoryItemDto)
        {
            if (newInventoryItemDto == null)
            {
                throw new ArgumentNullException();
            }
            var newInventoryItem = _mapper.Map<InventoryItem>(newInventoryItemDto);
            await _repository.Create(newInventoryItem);
            return newInventoryItem;
        }

        public async Task Update(int id, UpdateInventoryItemDto updateInventoryItemDto)
        {
            var itemToUpdate = await _repository.GetById(id);
            if (updateInventoryItemDto == null || itemToUpdate == null)
            {
                throw new ArgumentNullException();
            }

            _mapper.Map(updateInventoryItemDto, itemToUpdate);
            await _repository.Update(itemToUpdate);
        }
    }
}
