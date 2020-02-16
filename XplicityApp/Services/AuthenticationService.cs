using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using XplicityApp.Configurations;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Enums;
using XplicityApp.Infrastructure.Utils.Interfaces;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITimeService _timeService;
        public AuthenticationService(
            IOptions<AppSettings> appSettings,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ITimeService timeService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;
            _timeService = timeService;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var userToVerify = await _userManager.FindByEmailAsync(email);

            if (userToVerify == null)
            {
                return null;
            }

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var user = await _userManager.Users.Include(e => e.Employee).SingleAsync(x => x.Email == email);
                if(user.Employee.Status == EmployeeStatusEnum.Former)
                {
                    throw new InvalidOperationException();
                }
                user.Employee.Token = await CreateJwt(user);
                return user;
            }

            return null;
        }

        private async Task<string> CreateJwt(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim("role", role)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var durationTime = _timeService.GetCurrentTime().AddDays(14);

            var token = new JwtSecurityToken(
                "Issuer",
                "Issuer",
                claims,
                expires: durationTime,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
