using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(string email, string password);
    }
}
