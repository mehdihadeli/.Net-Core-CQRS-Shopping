using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Common.EF
{
    public interface IDataContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> CommitAsync();
    }
}