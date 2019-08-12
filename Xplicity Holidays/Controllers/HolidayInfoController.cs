using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayInfoController : ControllerBase
    {

        private readonly IHolidayInfoService _holidayInfoService;

        public HolidayInfoController(IHolidayInfoService holidayInfoService)
        {
            _holidayInfoService = holidayInfoService;
        }

        [HttpGet("{id}")]
        public IActionResult GetNumberOfHolidaysLeft(int id)
        {
            var holidaysLeft = _holidayInfoService.GetNumberOfHolidaysLeft(id);

            return Ok(holidaysLeft);
        }

        [HttpGet]
        public IActionResult GetAllEmployeesHolidaysLeft()
        {
            var holidaysLeft = _holidayInfoService.GetAllEmployeesHolidaysLeft();

            if (holidaysLeft == null)
                return NotFound();

            return Ok(holidaysLeft);
        }
    }
}