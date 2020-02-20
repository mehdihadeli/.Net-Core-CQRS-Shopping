using System;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure.Persistence;
using Shopping.Infrastructure.Persistence.Identity;

namespace Shopping.Tests.UnitTests.Common
{
    public static class ApplicationIdentityContextFactory
    {
        public static ApplicationIdentityDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationIdentityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationIdentityDbContext(options);

            context.Database.EnsureCreated();

            context.SaveChanges();

            return context;
        }

        public static void Destroy(ApplicationIdentityDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}