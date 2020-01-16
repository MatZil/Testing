using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
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