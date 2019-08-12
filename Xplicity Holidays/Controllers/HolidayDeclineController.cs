using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos.Holidays;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayDeclineController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;
        private readonly IMapper _mapper;

        public HolidayDeclineController(IHolidaysService holidaysService, IMapper mapper)
        {
            _holidaysService = holidaysService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> DeclineHoliday(int holidayId)
        {
            var getHolidayDto = await _holidaysService.GetById(holidayId);
            var updateHolidayDto = _mapper.Map<UpdateHolidayDto>(getHolidayDto);
            updateHolidayDto.Status = "Declined";
            await _holidaysService.Update(holidayId, updateHolidayDto);
            return Ok();
        }
    }
}