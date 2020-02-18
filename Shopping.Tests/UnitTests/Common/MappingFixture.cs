using System;
using AutoMapper;
using Shopping.Core.Mappings;
using Xunit;

namespace Shopping.Tests.UnitTests.Common
{
    public class MappingFixture : IDisposable
    {
        public IMapper Mapper { get; }
        public IConfigurationProvider ConfigurationProvider { get; }

        public MappingFixture()
        {
            ConfigurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfileConventions>();
                cfg.AddProfile<UserProfile>();
            });

            Mapper = ConfigurationProvider.CreateMapper();
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("MappingCollection")]
    public class MappingCollection : ICollectionFixture<MappingFixture>
    {
    }
}