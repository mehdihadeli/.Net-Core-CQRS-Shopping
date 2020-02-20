using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shopping.Tests.EndToEndTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class Delete
    {
        private readonly HttpClient _authenticatedClient;

        public Delete(TestHostFixture testHostFixture)
        {
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }

        [Fact]
        [AutoRollbackAttribute]
        public async Task Should_Delete_Customer_With_Valid_Id_And_Returns_NoContent_StatusCode()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var validId = 1;

            var response = await _authenticatedClient.DeleteAsync($"/api/customers/delete/{validId}");

            response.EnsureSuccessStatusCode().StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        [AutoRollbackAttribute]
        public async Task Should_Not_Delete_Customer_When_Id_Is_Invalid_And_Returns_NotFound_StatusCode()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var invalidId = 0;

            var response = await _authenticatedClient.DeleteAsync($"/api/customers/delete/{invalidId}");
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
