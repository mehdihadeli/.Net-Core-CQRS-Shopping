using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Redis
{
    public static class Extensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            RedisOptions options = new RedisOptions();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            configuration.GetSection(nameof(RedisOptions)).Bind(options);
            services.AddSingleton(options);
            
            services.AddDistributedRedisCache(x => 
            {
                x.Configuration = options.ConnectionString;
                x.InstanceName = options.InstanceName;
            });

            return services;
        }
    }
}