using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryCategoryController : ControllerBase
    {
        private readonly IInventoryCategoryService _inventoryCategoryService;

        public InventoryCategoryController(IInventoryCategoryService inventoryCategoryService)
        {
            _inventoryCategoryService = inventoryCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _inventoryCategoryService.GetAll();
            return Ok(categories);
        }
    }
}