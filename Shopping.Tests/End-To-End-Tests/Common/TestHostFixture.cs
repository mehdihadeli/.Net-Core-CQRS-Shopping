using System;
using System.Net.Http;
using Xunit;

namespace Shopping.Tests.EndToEndTests.Common
{
    /// <summary>
    /// One instance of this will be created per test collection.
    /// </summary>
    public class TestHostFixture : IDisposable
    {
        public HttpClient AnonymousClient { get; }
        public HttpClient AuthenticatedClient { get; }
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public TestHostFixture()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            AnonymousClient = _factory.GetAnonymousClient();
            AuthenticatedClient = _factory.GetAuthenticatedClientAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _factory?.Dispose();
            AnonymousClient?.Dispose();
            AuthenticatedClient?.Dispose();
        }
    }

    [CollectionDefinition("TestHostCollection")]
    public class TestHostCollection : ICollectionFixture<TestHostFixture>
    {
    }
}