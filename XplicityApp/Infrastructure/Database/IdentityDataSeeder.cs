using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using XplicityApp.Infrastructure.Database.Models;

namespace XplicityApp.Infrastructure.Database
{
    public static class IdentityDataSeeder
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            SeedRoles(roleManager, configuration);
            SeedUser(userManager, configuration);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            if (!roleManager.RoleExistsAsync(configuration.GetValue<string>("AdminData:RoleName")).Result)
            {
                var role = new IdentityRole {Name = configuration.GetValue<string>("AdminData:RoleName")};
                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole {Name = "Employee"};
                roleManager.CreateAsync(role).Wait();
            }
        }

        private static void SeedUser(UserManager<User> userManager, IConfiguration configuration)
        {
            if (userManager.GetUsersInRoleAsync("Admin").Result.Count <= 0)
            {
                var user = new User
                {
                    UserName = configuration.GetValue<string>("AdminData:AdminEmail"),
                    Email = configuration.GetValue<string>("AdminData:AdminEmail"),
                    EmployeeId = 1
                };
                var result = userManager.CreateAsync(user, configuration.GetValue<string>("AdminData:AdminPassword")).Result;
                
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, configuration.GetValue<string>("AdminData:RoleName")).Wait();
                }
            }
        }
    }
}
