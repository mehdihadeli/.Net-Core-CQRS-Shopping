using System;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure.Persistence.Shopping;

namespace Shopping.Tests.UnitTests.Common
{
    public static class ShoppingContextFactory
    {
        public static ShoppingDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ShoppingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingDbContext(options);

            context.Database.EnsureCreated();

            context.SaveChanges();

            return context;
        }

        public static void Destroy(ShoppingDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}