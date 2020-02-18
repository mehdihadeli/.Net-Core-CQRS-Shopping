using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Core.ViewModels;
using Shopping.Tests.EndToEndTests.Common;
using Shouldly;
using Xunit;

namespace Shopping.Tests.Controllers.Customer
{
    [Collection("TestHostCollection")]
    public class GetAll 
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _anonymousClient;
        private readonly HttpClient _authenticatedClient;

        public GetAll(TestHostFixture testHostFixture)
        {
            _factory = testHostFixture.Factory;
            _anonymousClient = testHostFixture.AnonymousClient;
            _authenticatedClient = testHostFixture.AuthenticatedClient;
        }

        [Fact]
        public async Task Should_Get_All_Customers()
        {
            //var client = await _factory.GetAuthenticatedClientAsync();

            var response = await _anonymousClient.GetAsync("/api/customers/getall");

            response.EnsureSuccessStatusCode();

            var vm = await E2ETestsUtilities.GetResponseContent<CustomersListVm>(response);
            vm.ShouldBeOfType<CustomersListVm>();
            vm.Customers.ShouldNotBeEmpty();
        }
    }
}
