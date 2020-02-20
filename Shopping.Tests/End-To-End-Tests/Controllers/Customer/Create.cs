using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Core.Commands;
using Shopping.Tests.EndToEndTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.EndToEndTests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class Create 
    {
        private readonly HttpClient _authenticatedClient;

        public Create(TestHostFixture testHostFixture)
        {
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }

        [Fact]
        [AutoRollbackAttribute]
        public async Task Should_Create_Customer_And_Return_NoContent_StatusCode()
        {
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
            var response = await _authenticatedClient.PostAsync($"/api/customers/create", content);

            response.EnsureSuccessStatusCode().StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}