using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _service;

        public HolidayConfirmController(IHolidayConfirmService service)
        {
            _service = service;
        }
        [HttpPost]
        [Produces(typeof(NewHolidayDto))]
        public IActionResult Post(NewHolidayDto newHolidayDto)
        {
            _service.RequestClientApproval(newHolidayDto);

            return Ok();
        }
    }
}