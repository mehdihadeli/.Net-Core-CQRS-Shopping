using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Infrastructure.Persistence.Identity;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping
{
    public static class MigrationManager
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                try
                {
                    MigrateShopping(scope);
                    MigrateIdentity(scope);
                }
                catch (Exception)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }

            return webHost;
        }

        private static void MigrateIdentity(IServiceScope scope)
        {
            var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
            identityDbContext.Database.EnsureCreated();
            identityDbContext.Database.Migrate();
        }

        private static void MigrateShopping(IServiceScope scope)
        {
            var shoppingDbContext = scope.ServiceProvider.GetRequiredService<ShoppingDbContext>();
            shoppingDbContext.Database.EnsureCreated();
            shoppingDbContext.Database.Migrate();
        }
    }
}