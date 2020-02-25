using System;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopping.Infrastructure.Persistence;

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
                    MigrateIdentityServer(scope);
                }
                catch (Exception)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }

            return webHost;
        }

        private static void MigrateIdentityServer(IServiceScope scope)
        {
            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            persistedGrantDbContext.Database.Migrate();

            var env = scope.ServiceProvider.GetService<IWebHostEnvironment>();

            // if (env.IsEnvironment("Test") == false)
            // {
                var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();
            //}
        }

        private static void MigrateIdentity(IServiceScope scope)
        {
            var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
            identityDbContext.Database.Migrate();
        }

        private static void MigrateShopping(IServiceScope scope)
        {
            var shoppingDbContext = scope.ServiceProvider.GetRequiredService<ShoppingDbContext>();
            shoppingDbContext.Database.Migrate();
        }
    }
}