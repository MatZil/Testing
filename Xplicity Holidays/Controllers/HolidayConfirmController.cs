using System.Threading.Tasks;
using AutoMapper;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayConfirmController : ControllerBase
    {
        private readonly IHolidayConfirmService _service;
        private readonly IRepository<Holiday> _repositoryHolidays;
        private readonly IMapper _mapper;
        private readonly IFileDeliveryService _deliveryService;
        private readonly IConfiguration _configuration;
        public HolidayConfirmController(IHolidayConfirmService service, IRepository<Holiday> repositoryHolidays, IMapper mapper,
                                        IFileDeliveryService deliveryService, IConfiguration configuration)
        {
            _service = service;
            _repositoryHolidays = repositoryHolidays;
            _mapper = mapper;
            _deliveryService = deliveryService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RequestConfirmationFromClient(NewHolidayDto newHolidayDto)
        {
            var holiday = _mapper.Map<Holiday>(newHolidayDto);
            var holidayId = await _repositoryHolidays.Create(holiday);

            await _service.RequestClientApproval(newHolidayDto, holidayId);

            await _service.CreateRequestPdf(newHolidayDto, holidayId);

            var path = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey) + @"\Pdfs\Requests";
            var fileName = $"Holiday_Request_{holidayId}.pdf";
            var result = _deliveryService.DeliverFile(path, fileName, "pdf", "request");

            return result;
        }

        [HttpPut]
        public async Task<IActionResult> RequestConfirmationFromAdmin(int holidayId)
        {
            await _service.RequestAdminApproval(holidayId, "Employee's client has confirmed this holiday.");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmHoliday(int holidayId)
        {
            await _service.CreateOrderPdf(holidayId);
            return Ok();
        }
    }
}