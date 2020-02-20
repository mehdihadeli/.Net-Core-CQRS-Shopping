using System;
using System.Threading.Tasks;
using Common.Authentication;
using Common.Authentication.Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Shopping.Core.Domains;
using Shopping.Infrastructure.Persistence.Identity.Configurations;

namespace Shopping.Infrastructure.Persistence
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>,
        IIdentityDataContext
    {
        private IDbContextTransaction _transaction;

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

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