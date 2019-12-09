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
        private readonly IHolidayConfirmService _confirmationService;
        private readonly IHolidaysService _holidaysService;

        public HolidayConfirmController(IHolidayConfirmService confirmationService, IHolidaysService holidaysService)
        {
            _confirmationService = confirmationService;
            _holidaysService = holidaysService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            if (!await _confirmationService.IsValid(newHolidayDto))
            {
                return BadRequest();
            }

            var holidayId = await _holidaysService.Create(newHolidayDto);

            await _confirmationService.RequestClientApproval(holidayId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            if (!await _confirmationService.IsValid(holidayId))
            {
                return BadRequest();
            }

            await _confirmationService.ConfirmHoliday(holidayId);

            await _confirmationService.GenerateFilesAndNotify(holidayId);

            return Ok();
        }
    }
}