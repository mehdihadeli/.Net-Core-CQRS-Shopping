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
        public CustomWebApplicationFactory<Startup> Factory { get; }

        public TestHostFixture()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
            AnonymousClient = Factory.GetAnonymousClient();
            //AuthenticatedClient = Factory.GetAuthenticatedClientAsync();
        }

        public void Dispose()
        {
            AnonymousClient?.Dispose();
            AuthenticatedClient?.Dispose();
            Factory?.Dispose();
        }
    }
    
    [CollectionDefinition("TestHostCollection")]
    public class TestHostCollection : ICollectionFixture<TestHostFixture>
    {
    }
}