using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Static_Files;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayClientController : ControllerBase
    {
        private readonly IHolidayConfirmService _confirmationService;

        public HolidayClientController(IHolidayConfirmService confirmationService)
        {
            _confirmationService = confirmationService;
        }

        [HttpPost]
        public async Task<IActionResult> RequestConfirmationFromAdmin([FromForm]int holidayId, [FromForm]int confirmerId)
        {
            await _confirmationService.RequestAdminApproval(holidayId, EmployeeClientStatus.CLIENT_CONFIRMED, confirmerId);

            return Ok();
        }
    }
}