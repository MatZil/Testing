using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
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

        [HttpGet]
        public async Task<IActionResult> RequestConfirmationFromAdmin(int holidayId)
        {
            await _confirmationService.RequestAdminApproval(holidayId, "Employee's client has confirmed this holiday.");

            return Ok();
        }
    }
}