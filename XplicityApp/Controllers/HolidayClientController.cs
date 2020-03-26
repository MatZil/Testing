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
    }
}