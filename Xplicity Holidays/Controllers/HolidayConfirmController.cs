using System.Threading.Tasks;
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
        public async Task<IActionResult> ConfirmFromClient(NewHolidayDto newHolidayDto)
        {
            await _service.RequestClientApproval(newHolidayDto);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmFromAdmin(int holidayId)
        {
            await _service.RequestAdminApproval(holidayId, "Employee's client has confirmed this holiday.");

            return Ok();
        }
    }
}