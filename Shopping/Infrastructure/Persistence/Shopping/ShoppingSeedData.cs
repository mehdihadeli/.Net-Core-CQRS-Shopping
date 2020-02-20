using System.Threading.Tasks;

namespace Shopping.Infrastructure.Persistence
{
    public static class ShoppingSeedData
    {
        public static Task SeedAsync()
        {
            return Task.CompletedTask;
        }
    }
}
