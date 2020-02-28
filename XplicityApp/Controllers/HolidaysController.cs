using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;
        private readonly ITimeService _timeService;

        public HolidaysController(IHolidaysService holidaysService, ITimeService timeService)
        {
            _holidaysService = holidaysService;
            _timeService = timeService;
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

        [HttpGet("is-free-workday")]
        [Produces(typeof(bool))]
        public IActionResult IsFreeWorkday(DateTime date)
        {
            return Ok(_timeService.IsFreeWorkDay(date));
        }
    }
}
