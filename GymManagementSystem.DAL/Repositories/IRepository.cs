using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> GetAllIncludingAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includes);


        Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellation = default);

        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(TEntity entity);
    }
}
