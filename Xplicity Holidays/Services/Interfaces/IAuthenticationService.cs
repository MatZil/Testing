using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Infrastructure.Repositories;

namespace Xplicity_Holidays.Services.Interfaces
{
    public interface IAuthService
    {
        Employee Authenticate(IEmployeeRepository repository, string email, string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        string CreateJwt(Employee employee);
    }
}
