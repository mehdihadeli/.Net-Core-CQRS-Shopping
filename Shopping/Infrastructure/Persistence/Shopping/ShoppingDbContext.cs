using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Shopping.Core.Domains;
using Shopping.Core.Services;
using Shopping.Infrastructure.Persistence.Shopping.Configurations;

namespace Shopping.Infrastructure.Persistence
{
    public class ShoppingDbContext : DbContext, IShoppingDataContext
    {
        private readonly DbContextOptions<ShoppingDbContext> _options;
        private readonly ICurrentUserService _currentUserService;
        private IDbContextTransaction _transaction;

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : base(options)
        {
            _options = options;
        }

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _options = options;
            _currentUserService = currentUserService;
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Region> Region { get; set; }

        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Territory> Territories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService?.UserId ?? 0;
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService?.UserId ?? 0;
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new ShipperConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new TerritoryConfiguration());
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public int Commit()
        {
            try
            {
                var saveChanges = SaveChanges();
                _transaction.Commit();
                return saveChanges;
            }
            finally
            {
                _transaction.Dispose();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public Task<int> CommitAsync()
        {
            try
            {
                var saveChangesAsync = SaveChangesAsync();
                _transaction.Commit();
                return saveChangesAsync;
            }
            finally
            {
                _transaction.Dispose();
            }
        }
    }
}