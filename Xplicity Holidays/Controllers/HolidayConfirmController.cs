using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Enums;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _confirmationService;
        private readonly IConfiguration _configuration;
        private readonly IHolidaysService _holidaysService;
        private readonly IDocxGeneratorService _docxGeneratorService;

        public HolidayConfirmController(IHolidayConfirmService confirmationService, IConfiguration configuration,
                                        IHolidaysService holidaysService, IDocxGeneratorService docxGeneratorService)
        {
            _confirmationService = confirmationService;
            _configuration = configuration;
            _holidaysService = holidaysService;
            _docxGeneratorService = docxGeneratorService;
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

            await _docxGeneratorService.GenerateHolidayDocx(holidayId, HolidayDocumentType.Request);

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

            await _docxGeneratorService.GenerateHolidayDocx(holidayId, HolidayDocumentType.Order);

            await _confirmationService.Notify(holidayId, EmployeeRoleEnum.Administrator);

            await _confirmationService.Notify(holidayId, EmployeeRoleEnum.Regular);

            return Ok();
        }
    }
}