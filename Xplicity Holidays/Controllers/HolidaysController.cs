using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public HolidaysController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        // GET: api/holidays
        [HttpGet]
        [Produces(typeof(GetHolidayDto[]))]
        public async Task<IActionResult> Get()
        {
            var holidays = await _holidaysService.GetAll();
            return Ok(holidays);
        }

        // GET: api/holidays/5
        [HttpGet("{id}")]
        [Produces(typeof(GetHolidayDto))]
        public async Task<IActionResult> Get(int id)
        {
            var holiday = await _holidaysService.GetById(id);

            if (holiday == null)
            {
                return NotFound();
            }

            return Ok(holiday);
        }

        // PUT: api/holidays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateHolidayDto newHoliday)
        {
            await _holidaysService.Update(id, newHoliday);

            return NoContent();
        }

        // POST: api/holidays
        [HttpPost]
        public async Task<IActionResult> Post(NewHolidayDto newHoliday)
        {
            await _holidaysService.Create(newHoliday);

            return Ok();
        }

        // DELETE: api/holidays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _holidaysService.Delete(id);

            return NoContent();
        }
    }
}
