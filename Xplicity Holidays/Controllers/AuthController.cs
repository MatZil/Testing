using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xplicity_Holidays.Dtos;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateDto request)
        {
            if (request.Email != null && request.Password != null)
            {
                var result = await _authenticationService.Authenticate(request.Email, request.Password);
                
                if (result != null)
                {
                    return Ok(new
                    {
                        result.EmployeeId,
                        result.Employee.Token
                    });
                }

                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _authenticationService.GetAllRoles();
            return Ok(roles);
        }
    }
}