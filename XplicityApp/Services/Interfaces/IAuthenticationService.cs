using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Authenticate(string email, string password);
        Task<List<IdentityRole>> GetAllRoles();
    }
}
