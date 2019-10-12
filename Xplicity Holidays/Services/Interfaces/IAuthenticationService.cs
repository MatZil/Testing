using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> Authenticate(string email, string password);
        Task<List<IdentityRole>> GetAllRoles();
    }
}
