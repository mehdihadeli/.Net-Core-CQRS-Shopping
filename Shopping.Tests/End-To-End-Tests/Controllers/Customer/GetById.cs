using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Core.ViewModels;
using Shopping.Tests.EndToEndTests.Common;
using Xunit;

namespace Shopping.Tests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class GetById 
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _anonymousClient;
        private readonly HttpClient _authenticatedClient;

        public GetById(TestHostFixture testHostFixture)
        {
            _factory = testHostFixture.Factory;
            _anonymousClient = testHostFixture.AnonymousClient;
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }

        [Fact]
        public async Task Should_Get_A_Customer_With_Valid_Id()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var validId = 1;

            var response = await _anonymousClient.GetAsync($"/api/customers/get/{validId}");

            response.EnsureSuccessStatusCode();

            var customer = await E2ETestsUtilities.GetResponseContent<CustomerDetailVm>(response);

            Assert.Equal(validId, customer.Id);
        }

        [Fact]
        public async Task Should_Not_Get_A_Customer_With_Invalid_Id_And_Returns_NotFoundStatusCode()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();
            
            var invalidId = 0;

            var response = await _anonymousClient.GetAsync($"/api/customers/get/{invalidId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
