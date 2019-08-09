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
        private readonly IHolidaysService _service;

        public HolidaysController(IHolidaysService service)
        {
            _service = service;
        }

        // GET: api/holidays
        [HttpGet]
        [Produces(typeof(GetHolidayDto[]))]
        public async Task<IActionResult> Get()
        {
            var holidays = await _service.GetAll();
            return Ok(holidays);
        }

        // GET: api/holidays/5
        [HttpGet("{id}")]
        [Produces(typeof(GetHolidayDto))]
        public async Task<IActionResult> Get(int id)
        {
            var holiday = await _service.GetById(id);

            if (holiday == null)
            {
                return NotFound();
            }

            return Ok(holiday);
        }

        // PUT: api/holidays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewHolidayDto newHoliday)
        {
            await _service.Update(id, newHoliday);

            return NoContent();
        }

        // POST: api/holidays
        [HttpPost]
        [Produces(typeof(NewHolidayDto))]
        public async Task<IActionResult> Post(NewHolidayDto newHoliday)
        {
            var createdHoliday = await _service.Create(newHoliday);

            return Ok(createdHoliday);
        }

        // DELETE: api/holidays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);

            return NoContent();
        }
    }
}
