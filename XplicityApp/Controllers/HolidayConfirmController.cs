using Microsoft.AspNetCore.Mvc;
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
        private const string HolidayConfirmedMessage = "Holiday confirmed.";
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

            return Ok(HolidayConfirmedMessage);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            try
            {
                await _confirmationService.ValidateHolidayConfirmationReadiness(holidayId);

                await _confirmationService.ConfirmHoliday(holidayId);

                await _confirmationService.GenerateFilesAndNotify(holidayId);
            }
            catch (InvalidOperationException exception)
            {
                return BadRequest(exception.Message);
            }

            return Ok(HolidayConfirmedMessage);
        }
    }
}