using System.Threading.Tasks;
using Shopping.Core.Domains;
using Shopping.Core.Services;

namespace Shopping.Infrastructure.Persistence.Identity
{
    public static class IdentitySeedData
    {
        public static async Task SeedAsync(IUserManagerService userManager)
        {
            var user = new ApplicationUser {UserName = "mehdi", Email = "mehdi@yahoo.com"};

            if (await userManager.CheckForExistingUser(user.UserName) == false)
            {
                await userManager.CreateUser(user, "123");
            }
        }
    }
}