using Microsoft.AspNetCore.Identity;

namespace XplicityApp.Infrastructure.Database.Models
{
    public class User : IdentityUser
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}
