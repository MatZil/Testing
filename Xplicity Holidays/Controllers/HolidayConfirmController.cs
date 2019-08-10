using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public HolidayConfirmController(IHolidayConfirmService service, IRepository<Holiday> repositoryHolidays, IMapper mapper)
        {
            _service = service;
            _repositoryHolidays = repositoryHolidays;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmFromClient(NewHolidayDto newHolidayDto)
        {
            var holiday = _mapper.Map<Holiday>(newHolidayDto);
            var holidayId = await _repositoryHolidays.Create(holiday);
            await _service.RequestClientApproval(newHolidayDto, holidayId);
            await _service.CreatePdf(newHolidayDto, holidayId);
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