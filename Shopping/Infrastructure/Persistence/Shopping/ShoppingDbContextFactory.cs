using Microsoft.EntityFrameworkCore;

namespace Shopping.Infrastructure.Persistence.Shopping
{
    public class ShoppingDbContextFactory : DesignTimeDbContextFactoryBase<ShoppingDbContext>
    {
        protected override ShoppingDbContext CreateNewInstance(DbContextOptions<ShoppingDbContext> options)
        {
            return new ShoppingDbContext(options);
        }
    }
}