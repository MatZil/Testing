using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Dtos.Tags;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class InventoryItemService : IInventoryItemService
    {
        private readonly IInventoryItemRepository _repository;
        private readonly IInventoryItemTagsRepository _inventoryItemTagsRepository;
        private readonly ITagsRepository _tagRepository;
        private readonly IMapper _mapper;

        public InventoryItemService(
            IInventoryItemRepository repository,
            IInventoryItemTagsRepository inventoryItemTagsRepository,
            ITagsRepository tagRepository,
            IMapper mapper
            )
        {
            _repository = repository;
            _inventoryItemTagsRepository = inventoryItemTagsRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        public async Task<GetInventoryItemDto> GetById(int id)
        {
            var inventoryItem = await _repository.GetById(id);
            var inventoryItemDto = _mapper.Map<GetInventoryItemDto>(inventoryItem);

            if (inventoryItemDto != null)
            {
                inventoryItemDto.Tags = await GetTagsListByItemId(inventoryItemDto.Id);
            }

            return inventoryItemDto;
        }

        public async Task<ICollection<GetInventoryItemDto>> GetAll()
        {
            var inventoryItems = await _repository.GetAll();
            var inventoryItemsDto = _mapper.Map<GetInventoryItemDto[]>(inventoryItems);

            foreach (var inventoryItemDto in inventoryItemsDto)
            {
                inventoryItemDto.Tags = await GetTagsListByItemId(inventoryItemDto.Id);

                foreach (var inventoryItem in inventoryItems)
                {
                    if (inventoryItemDto.EmployeeId == inventoryItem.EmployeeId)
                    {
                        inventoryItemDto.AssignedTo = inventoryItem.Employee.Name + " " + inventoryItem.Employee.Surname;
                        break;
                    }
                }

                if (inventoryItemDto.AssignedTo == null)
                {
                    inventoryItemDto.AssignedTo = "Office";
                }
            }
            return inventoryItemsDto;
        }

        public async Task<ICollection<GetInventoryItemDto>> GetByEmployeeId(int employeeId)
        {
            var inventoryItems = await _repository.GetByEmployeeId(employeeId);
            var inventoryItemsDto = _mapper.Map<GetInventoryItemDto[]>(inventoryItems);

            foreach (var inventoryItemDto in inventoryItemsDto)
            {
                inventoryItemDto.Tags = await GetTagsListByItemId(inventoryItemDto.Id);
            }

            return inventoryItemsDto;
        }

        public async Task<InventoryItem> Create(NewInventoryItemDto newInventoryItemDto)
        {
            if (newInventoryItemDto == null)
            {
                throw new ArgumentNullException();
            }

            var newInventoryItem = _mapper.Map<InventoryItem>(newInventoryItemDto);
            var inventoryItemId = await _repository.Create(newInventoryItem);

            foreach (var tagId in newInventoryItemDto.TagIds)
            {
                await _inventoryItemTagsRepository.Create(new InventoryItemTag()
                {
                    InventoryItemId = inventoryItemId,
                    TagId = tagId
                });
            }

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

            foreach (var inventoryItemTag in updateInventoryItemDto.Tags)
            {
                if (await _inventoryItemTagsRepository.FindByInventoryItemIdAndTagId(itemToUpdate.Id, inventoryItemTag.Id) is null)
                {
                    await _inventoryItemTagsRepository.Create(new InventoryItemTag()
                    {
                        InventoryItemId = itemToUpdate.Id,
                        TagId = inventoryItemTag.Id
                    });
                }
            }

            var allCurrentItemTags = await _inventoryItemTagsRepository.FindAllByInventoryItemId(itemToUpdate.Id);

            foreach (var currentItemTag in allCurrentItemTags)
            {
                var tagDto = _mapper.Map<TagDto>(await _tagRepository.GetById(currentItemTag.TagId));

                if (!updateInventoryItemDto.Tags.Any(x => x.Id == tagDto.Id))
                {
                    await _inventoryItemTagsRepository.Delete(await _inventoryItemTagsRepository.GetById(currentItemTag.Id));
                }
            }
        }

        private async Task<List<TagDto>> GetTagsListByItemId(int itemId)
        {
            var tags = new List<TagDto>();

            var InventoryItemTags = await _inventoryItemTagsRepository.FindAllByInventoryItemId(itemId);

            foreach (var inventoryItemTag in InventoryItemTags)
            {
                tags.Add(_mapper.Map<TagDto>(await _tagRepository.GetById(inventoryItemTag.TagId)));
            };

            return tags;
        }
    }
}
