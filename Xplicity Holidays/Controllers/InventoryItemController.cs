using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Inventory;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryItemService _inventoryItemService;

        public InventoryItemController(IInventoryItemService inventoryItemService)
        {
            _inventoryItemService = inventoryItemService;
        }

        [HttpGet("GetByEmployeeId/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var items = await _inventoryItemService.GetByEmployeeId(id);
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _inventoryItemService.GetAll();
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewInventoryItemDto newInventoryItemDto)
        {
            var newInventoryItem = await _inventoryItemService.Create(newInventoryItemDto);
            return CreatedAtRoute(new {id = newInventoryItem.Id}, newInventoryItem);
        }
    }
}