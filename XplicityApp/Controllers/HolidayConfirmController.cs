using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Services.Interfaces;
using XplicityApp.Services.Validations.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _confirmationService;
        private readonly IHolidayValidationService _holidayValidationService;
        private readonly IHolidaysService _holidaysService;
        private readonly ILogger<HolidayConfirmController> _logger;

        public HolidayConfirmController(
            IHolidayConfirmService confirmationService,
            IHolidayValidationService holidayValidationService,
            IHolidaysService holidaysService,
            ILogger<HolidayConfirmController> logger)
        {
            _confirmationService = confirmationService;
            _holidayValidationService = holidayValidationService;
            _holidaysService = holidaysService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            try
            {
                await _holidayValidationService.ValidateNewHolidayConfirmationReadiness(newHolidayDto);
                var holidayId = await _holidaysService.Create(newHolidayDto);
                await _confirmationService.RequestClientApproval(holidayId);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmHoliday(int holidayId, int confirmerId)
        {
            {
                _logger.LogInformation($"Holiday confirm request received for holiday id:{holidayId}");
                try
                {
                    await _holidayValidationService.ValidateHolidayConfirmationReadiness(holidayId, confirmerId);
                    await _confirmationService.ConfirmHoliday(holidayId, confirmerId);
                    await _confirmationService.GenerateFilesAndNotify(holidayId);
                }
                catch (InvalidOperationException exception)
                {
                    _logger.LogError($"Holiday confirm request failed with: {exception.Message}");
                    return Ok(exception.Message);
                }

                return Ok();
            }
        }
    }
}