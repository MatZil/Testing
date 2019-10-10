using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xplicity_Holidays.Dtos;
using Xplicity_Holidays.Infrastructure.Database.Models;
using IAuthenticationService = Xplicity_Holidays.Services.Interfaces.IAuthenticationService;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService, UserManager<User> userManager)
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
                    return Ok(result.Employee);
                }

                return Unauthorized();
            }

            return BadRequest();
        }
    }
}