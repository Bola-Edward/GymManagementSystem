using GymManagementSystem.DAL.Models;
using GymManagementSystem.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }


        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellation = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellation);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllIncludingAsync(CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
        {
            return await ApplyIncludes(_dbSet.AsQueryable(), includes)
                         .AsNoTracking()
                         .ToListAsync(cancellationToken);
        }


        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default, bool trackChanges = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = ApplyIncludes(_dbSet.AsQueryable(), includes);


            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }


        public async Task<TEntity?> GetByIdIncludedDeletedAsync(int id, CancellationToken cancellation = default)
        {
            return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id, cancellation);
        }


        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellation);
        }


        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        }


        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }


        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }


        public async Task SoftDeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _dbSet.AsNoTracking().AnyAsync(predicate, cancellationToken);
        }

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }


        public async Task<int> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
