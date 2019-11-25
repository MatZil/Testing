using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly ITemplateGenerationService _templateGenerationService;

        public HolidayConfirmController(IHolidayConfirmService confirmationService, IConfiguration configuration, 
                                        IHolidaysService holidaysService, ITemplateGenerationService templateGenerationService)
        {
            _confirmationService = confirmationService;
            _configuration = configuration;
            _holidaysService = holidaysService;
            _templateGenerationService = templateGenerationService;
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

            await _templateGenerationService.GenerateHolidayDocx(holidayId, HolidayDocumentType.Request);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Templates\GeneratedTemplates\";
            var fileName = $"{holidayId}-Request{newHolidayDto.Type.ToString()}-{DateTime.Today.Date.ToShortDateString()}.docx";
            var stream = new FileStream(path + fileName, FileMode.Open);
            return File(stream, "application/docx", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            if (!await _confirmationService.IsValid(holidayId))
            {
                return BadRequest();
            }

            await _confirmationService.ConfirmHoliday(holidayId);

            await _templateGenerationService.GenerateHolidayDocx(holidayId, HolidayDocumentType.Order);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Templates\GeneratedTemplates\";
            var holidayDto = await _holidaysService.GetById(holidayId);
            var fileName = $"{holidayId}-Order{holidayDto.Type.ToString()}-{DateTime.Today.Date.ToShortDateString()}.docx";
            var stream = new FileStream(path + fileName, FileMode.Open);
            return File(stream, "application/docx", fileName);
        }
    }
}