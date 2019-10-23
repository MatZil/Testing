using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayDeclineController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public HolidayDeclineController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet]
        public async Task<IActionResult> DeclineHoliday(int holidayId)
        {
            var successful = await _holidaysService.Decline(holidayId);

            if(!successful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}