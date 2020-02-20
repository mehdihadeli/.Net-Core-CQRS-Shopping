using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Tests.EndToEndTests.Common
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            var server = base.CreateServer(builder);

            //https://github.com/dotnet/aspnetcore/issues/10134
            //https://github.com/dotnet/aspnetcore/issues/18001
            server.PreserveExecutionContext = true; // this line fixed the issue with nested transaction with HttpClient

            var sp = server.Services;
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var shoppingDataContext = scopedServices.GetRequiredService<ShoppingDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
            try
            {
                server.Host.MigrateDatabase();
                // Seed the database with test data.
                E2ETestsUtilities.InitializeDbForTests(shoppingDataContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the " +
                                    $"database with test messages. Error: {ex.Message}");
            }

            return server;
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            return await GetAuthenticatedClientAsync("Test", "Test");
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        {
            try
            {
                var client = CreateClient();

                var token = await GetAccessTokenAsync(client, userName, password);

                client.SetBearerToken(token);

                return client;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string userName, string password)
        {
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:8000");

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "Shopping.ResourcePassword.Client",
                ClientSecret = "secret",
                Scope = "Shopping.API openid profile",
                UserName = userName,
                Password = password
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }
    }
}