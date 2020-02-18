using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shopping.Infrastructure.Persistence.Identity
{
    public class ApplicationIdentityDbContextFactory : DesignTimeDbContextFactoryBase<ApplicationIdentityDbContext>
    {
        protected override ApplicationIdentityDbContext CreateNewInstance(
            DbContextOptions<ApplicationIdentityDbContext> options)
        {
            return new ApplicationIdentityDbContext(options);
        }
    }
}