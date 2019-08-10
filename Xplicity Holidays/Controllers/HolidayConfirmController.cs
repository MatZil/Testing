using System.Threading.Tasks;
using AutoMapper;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Infrastructure.Utils.Interfaces;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _confirmationService;
        private readonly IMapper _mapper;
        private readonly IFileDeliveryService _deliveryService;
        private readonly IConfiguration _configuration;
        private readonly IHolidaysService _holidaysService;

        public HolidayConfirmController(IHolidayConfirmService confirmationService, IMapper mapper, IFileDeliveryService deliveryService, 
            IConfiguration configuration, IHolidaysService holidaysService)
        {
            _confirmationService = confirmationService;
            _mapper = mapper;
            _deliveryService = deliveryService;
            _configuration = configuration;
            _holidaysService = holidaysService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            var holidayId = await _holidaysService.Create(newHolidayDto);

            await _confirmationService.RequestClientApproval(newHolidayDto, holidayId);

            await _confirmationService.CreateRequestPdf(newHolidayDto, holidayId);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdfs\Requests";
            var fileName = $"Holiday_Request_{holidayId}.pdf";
            var result = _deliveryService.DeliverFile(path, fileName, "pdf", "request");

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> RequestConfirmationFromAdmin(int holidayId)
        {
            await _confirmationService.RequestAdminApproval(holidayId, "Employee's client has confirmed this holiday.");

            return Ok();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> ConfirmHoliday(int holidayId)
        {
            var getHolidayDto = await _holidaysService.GetById(holidayId);
            var updateHolidayDto = _mapper.Map<UpdateHolidayDto>(getHolidayDto);
            updateHolidayDto.IsConfirmed = true;
            await _holidaysService.Update(holidayId, updateHolidayDto);

            await _confirmationService.CreateOrderPdf(holidayId);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdfs\Orders";
            var fileName = $"Holiday_Order_{holidayId}.pdf";
            var result = _deliveryService.DeliverFile(path, fileName, "pdf", "order");

            return result;
        }
    }
}