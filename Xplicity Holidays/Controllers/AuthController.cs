using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = Xplicity_Holidays.Services.Interfaces.IAuthenticationService;

namespace Xplicity_Holidays.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(IAuthenticationService authenticationService, UserManager<IdentityUser> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _authenticationService.Authenticate(_userManager, email, password);
            return Ok(result);
        }
    }
}