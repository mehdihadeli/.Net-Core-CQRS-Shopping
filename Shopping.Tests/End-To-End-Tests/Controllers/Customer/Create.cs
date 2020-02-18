using System.Net;
using System.Net.Http;
using Shopping.Core.Commands;
using Shopping.Tests.EndToEndTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.EndToEndTests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class Create 
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _anonymousClient;
        private readonly HttpClient _authenticatedClient;

        public Create(TestHostFixture testHostFixture)
        {
            _factory = testHostFixture.Factory;
            _anonymousClient = testHostFixture.AnonymousClient;
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }

        [Fact]
        [AutoRollbackAttribute]
        public void Should_Create_Customer_And_Return_NoContent_StatusCode()
        {
            // var client = await _factory.GetAuthenticatedClientAsync();
            var command = new CreateCustomerCommand
            {
                Id = 11,
                Address = "...",
                City = "Berlin",
                CompanyName = "BMW",
                ContactName = "Test Contract Name",
                ContactTitle = "Test Contract Title",
                Country = "Germany",
                Fax = "1212121",
                Phone = "4343434",
                PostalCode = "77777"
            };

            var content = E2ETestsUtilities.GetRequestContent(command);
            var response = _anonymousClient.PostAsync($"/api/customers/create", content).Result;

            response.EnsureSuccessStatusCode().StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}