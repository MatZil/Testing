using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Employee Authenticate(IEmployeeRepository repository, string email, string password);
        Task<IdentityUser> Authenticate(UserManager<IdentityUser> userManager, string email, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
