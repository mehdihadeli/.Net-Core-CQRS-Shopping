using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Infrastructure.Persistence
{
    public class ShoppingDbContextFactory : DesignTimeDbContextFactoryBase<ShoppingDbContext>
    {
        public ShoppingDbContextFactory() : base("DefaultConnection",
            typeof(ConfigurationContextDesignTimeFactory).GetTypeInfo().Assembly.GetName().Name)
        {
        }
        protected override ShoppingDbContext CreateNewInstance(DbContextOptions<ShoppingDbContext> options)
        {
            return new ShoppingDbContext(options);
        }
    }
}