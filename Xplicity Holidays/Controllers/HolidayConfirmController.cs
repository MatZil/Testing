using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Dtos.Holidays;
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

        public HolidayConfirmController(IHolidayConfirmService confirmationService, IConfiguration configuration, 
                                        IHolidaysService holidaysService)
        {
            _confirmationService = confirmationService;
            _configuration = configuration;
            _holidaysService = holidaysService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            {
            }
            var holidayId = await _holidaysService.Create(newHolidayDto);

            await _confirmationService.RequestClientApproval(holidayId);

            await _confirmationService.CreateRequestDocx(newHolidayDto, holidayId);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Templates\GeneratedTemplates\";
            var fileName = $"{holidayId}-Request{newHolidayDto.Type.ToString()}-{DateTime.Today.Date.ToShortDateString()}.docx";
            var stream = new FileStream(path + fileName, FileMode.Open);
            return File(stream, "application/docx", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            var getHolidayDto = await _holidaysService.GetById(holidayId);
            {
            updateHolidayDto.Status = "Confirmed";
            }

            await _confirmationService.CreateOrderDocx(holidayId);

            await _confirmationService.CreateOrderPdf(holidayId);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdfs\Orders\";
            var fileName = $"Holiday_Order_{holidayId}.pdf";
            var stream = new FileStream(path + fileName, FileMode.Open);
            return File(stream, "application/docx", fileName);
        }
    }
}