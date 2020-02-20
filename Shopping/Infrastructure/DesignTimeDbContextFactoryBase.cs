using System;
using System.Diagnostics;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shopping.Infrastructure
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> :
        IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _connectionStringName;
        private readonly string _migrationsAssemblyName = "Shopping";

        public DesignTimeDbContextFactoryBase(string connectionStringName, string migrationsAssemblyName)
        {
            _connectionStringName = connectionStringName;
            _migrationsAssemblyName = migrationsAssemblyName;
        }

        public TContext CreateDbContext(string[] args)
        {
            return Create(Directory.GetCurrentDirectory(), _connectionStringName,
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), _migrationsAssemblyName);
        }

        public TContext CreateWithConnectionStringName(string connectionStringName, string migrationsAssemblyName)
        {
            var basePath = AppContext.BaseDirectory;
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return Create(basePath, connectionStringName, environmentName, migrationsAssemblyName);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string connectionStringName, string envName,
            string migrationsAssemblyName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find a connection string named 'default'.");
            }

            return Create(connectionString, migrationsAssemblyName);
        }

        private TContext Create(string connectionString, string migrationsAssemblyName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"Connection string '{_connectionStringName}' is null or empty.",
                    nameof(connectionString));
            }

            Console.WriteLine(
                $"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(connectionString,
                sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationsAssemblyName));

            DbContextOptions<TContext> options = optionsBuilder.Options;

            return CreateNewInstance(options);
        }
    }
}