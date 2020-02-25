using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Shopping.Core.Domains;
using Shopping.Infrastructure;
using Shopping.Infrastructure.Persistence;

namespace Shopping.Tests.EndToEndTests.Common
{
    public class E2ETestsUtilities
    {
        public static StringContent GetRequestContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(stringResponse);

            return result;
        }

        public static void InitializeDbForTests(ShoppingDbContext context)
        {
            if (context.Customers.Any() == false)
            {
                context.Customers.Add(new Customer
                {
                    Id = 1,
                    Address = "test address",
                    City = "Berlin",
                    CompanyName = "Benz",
                    ContactName = "Test",
                    ContactTitle = "Test",
                    Country = "Germany",
                    Fax = "32323232",
                    Phone = "333333",
                    PostalCode = "44444"
                });
            }

            if (context.Products.Any() == false)
            {
                var supplier1 = new Supplier
                {
                    CompanyName = "Benz",
                    ContactName = "Test",
                    ContactTitle = "Test",
                    Address = "Test",
                    City = "Germany",
                    PostalCode = "24322",
                    Fax = "",
                    Phone = "232323",
                    HomePage = ""
                };

                var category1 = new Category
                {
                    CategoryName = "Test Category",
                    Description = "..."
                };

                context.Products.Add(
                    new Product
                    {
                        ProductName = "Maclaren",
                        Supplier = supplier1,
                        Category = category1,
                        QuantityPerUnit = "1",
                        UnitPrice = 15558.00m,
                        UnitsInStock = 2,
                        UnitsOnOrder = 0,
                        ReorderLevel = 10,
                        Discontinued = false
                    });
            }

            context.SaveChanges();
        }

        public static void InitializeIdentityServerForTests(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }

        public static void InitializeIdentityUserForTests(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            IdentitySeedData.SeedAsync(userManager, roleManager).Wait();
        }
    }
}