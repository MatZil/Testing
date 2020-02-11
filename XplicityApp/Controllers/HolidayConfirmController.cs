using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.Holidays;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _confirmationService;
        private readonly IHolidaysService _holidaysService;
        private readonly ILogger<HolidayConfirmController> _logger;

        public HolidayConfirmController(IHolidayConfirmService confirmationService,
            IHolidaysService holidaysService,
            ILogger<HolidayConfirmController> logger)
        {
            _confirmationService = confirmationService;
            _holidaysService = holidaysService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            try
            {
                await _confirmationService.ValidateNewHolidayConfirmationReadiness(newHolidayDto);

                var holidayId = await _holidaysService.Create(newHolidayDto);

                await _confirmationService.RequestClientApproval(holidayId);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok();
        }

        [HttpGet("{holidayId}")]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            _logger.LogInformation($"Holiday confirm request received for holiday id:{holidayId}");
            try
            {
                await _confirmationService.ValidateHolidayConfirmationReadiness(holidayId);

                await _confirmationService.ConfirmHoliday(holidayId);

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