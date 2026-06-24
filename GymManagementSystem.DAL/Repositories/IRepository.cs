using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellation = default);

        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellation = default);

        Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellation = default);

        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
