using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Shopping.Core.Domains;

namespace Shopping.Infrastructure.Persistence
{
    public static class IdentitySeedData
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            //Create admin role
            var adminRole = roleManager.FindByNameAsync("admin").Result;
            if (adminRole == null)
            {
                adminRole = new ApplicationRole {Name = "admin"};
                roleManager.CreateAsync(adminRole).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.view")).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.create")).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "user.modify")).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.view")).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.create")).Wait();
                roleManager.AddClaimAsync(adminRole, new Claim("permission", "role.modify")).Wait();
            }

            //Create default admin user
            if (!userManager.Users.Any())
            {
                var newUser = new ApplicationUser()
                {
                    UserName = "Test",
                    FirstName = "Test",
                    LastName = "Test",
                    Email = "mehdi@yahoo.com",
                    DateOfBirth = DateTime.Parse("1989-10-03"),
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };

                var task1 = userManager.CreateAsync(newUser, "Test").Result;
                var task2 = userManager.SetLockoutEnabledAsync(newUser, false).Result;

                //Assign admin role to user
               await userManager.AddToRoleAsync(newUser, "admin");
            }
        }
    }
}