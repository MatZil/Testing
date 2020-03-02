using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XplicityApp.Dtos.Inventory;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InventoryItemsController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemsController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> Get(int employeeId)
        {
            var items = await _inventoryItemService.GetByEmployeeId(employeeId);
            return Ok(items);
        }

        [HttpGet]
        [Produces(typeof(GetInventoryItemDto[]))]
        public async Task<IActionResult> GetByItemStatus(bool showArchivedInventory)
        {
            var items = await _inventoryItemService.GetByItemStatus(showArchivedInventory);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewInventoryItemDto newInventoryItemDto)
        {
            var newInventoryItem = await _inventoryItemService.Create(newInventoryItemDto);
            return CreatedAtRoute(new {id = newInventoryItem.Id}, newInventoryItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateInventoryItemDto updateInventoryItemDto)
        {
            await _inventoryItemService.Update(id, updateInventoryItemDto);
            return NoContent();
        }
    }
}