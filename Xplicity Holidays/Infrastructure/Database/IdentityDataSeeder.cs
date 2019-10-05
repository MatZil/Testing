using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Security.Policy;

namespace Xplicity_Holidays.Infrastructure.Database
{
    public class IdentityDataSeeder
    {


        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            SeedRoles(roleManager, configuration);
            SeedUser(userManager, configuration);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            if (!roleManager.RoleExistsAsync(configuration.GetValue<string>("AdminData:RoleName")).Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = configuration.GetValue<string>("AdminData:RoleName");
                IdentityResult result = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUser(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            if (userManager.FindByEmailAsync(configuration.GetValue<string>("AdminData:AdminEmail")).Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = configuration.GetValue<string>("AdminData:AdminEmail");
                user.Email = configuration.GetValue<string>("AdminData:AdminEmail");
                IdentityResult result = userManager.CreateAsync(user, configuration.GetValue<string>("AdminData:AdminPassword")).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, configuration.GetValue<string>("AdminData:RoleName")).Wait();
                }
            }
        }
    }
}
