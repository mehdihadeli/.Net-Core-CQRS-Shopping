using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Infrastructure.Persistence
{
    public class ApplicationIdentityDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationIdentityDbContext>
    {
        public ApplicationIdentityDbContextFactory() : base("DefaultConnection",
            typeof(ConfigurationContextDesignTimeFactory).GetTypeInfo().Assembly.GetName().Name)
        {
        }

        protected override ApplicationIdentityDbContext CreateNewInstance(
            DbContextOptions<ApplicationIdentityDbContext> options)
        {
            return new ApplicationIdentityDbContext(options);
        }
    }
}