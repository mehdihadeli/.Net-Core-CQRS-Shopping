using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Core.Commands.Seed;

namespace Shopping
{
    public static class SeedManager
    {
        public static async Task<IWebHost> SeedDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                try
                {
                    await InitIdentityContext(scope);
                    await InitShoppingContext(scope);
                }
                catch (Exception)
                {
                    //Log errors or do anything you think it's needed
                    throw;
                }
            }

            return webHost;
        }

        private static async Task InitIdentityContext(IServiceScope scope)
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedIdentityCommand(), default);
            await mediator.Send(new SeedIdentityServerCommand(), default);
        }

        private static async Task InitShoppingContext(IServiceScope scope)
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new SeedShoppingCommand(), default);
        }
    }
}