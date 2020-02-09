using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet]
        [Produces(typeof(GetHolidayDto[]))]
        [Route("GetByStatus")]
        public async Task<IActionResult> GetByEmployeeStatus(EmployeeStatusEnum employeeStatus)
        {
            var holidays = await _holidaysService.GetByEmployeeStatus(employeeStatus);
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

        // DELETE: api/holidays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var successful = await _holidaysService.Delete(id);

            if(!successful)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
