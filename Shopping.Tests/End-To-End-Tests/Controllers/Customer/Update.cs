using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Core.Commands;
using Shopping.Tests.EndToEndTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class Update 
    {
        private readonly HttpClient _authenticatedClient;

        public Update(TestHostFixture testHostFixture)
        {
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }


        [Fact]
        [AutoRollbackAttribute]
        public async Task Should_Update_Customer_When_Id_Is_Valid_And_Return_NoContentStatusCode()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var command = new UpdateCustomerCommand
            {
                Id = 1,
                Address = "...",
                City = "Berlin",
                CompanyName = "Benz",
                ContactName = "Test",
                ContactTitle = "Test",
                Country = "Germany",
                Fax = "32323232",
                Phone = "333333",
                PostalCode = "44444"
            };
            var content = E2ETestsUtilities.GetRequestContent(command);

            var response = await _authenticatedClient.PutAsync($"/api/customers/update/{command.Id}", content);

            response.EnsureSuccessStatusCode().StatusCode.ShouldBe(HttpStatusCode.NoContent);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        [AutoRollbackAttribute]
        public async Task Should_Not_Update_Customer_When_Id_Is_Invalid_And_Return_NotFoundStatusCode()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var invalidCommand = new UpdateCustomerCommand
            {
                Id = 0,
                Address = "...",
                City = "Berlin",
                CompanyName = "Benz",
                ContactName = "Test",
                ContactTitle = "Test",
                Country = "Germany",
                Fax = "32323232",
                Phone = "333333",
                PostalCode = "44444"
            };
            var content = E2ETestsUtilities.GetRequestContent(invalidCommand);

            var response = await _authenticatedClient.PutAsync($"/api/customers/update/{invalidCommand.Id}", content);
            
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
