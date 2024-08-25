using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TradeCore.AuthService.Domain.BaseEntity;

namespace TradeCore.AuthService.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
    {
        public readonly DbSet<TEntity> _entities;
        protected readonly DbContext _dbContext;
        private readonly ILogger<GenericRepository<TEntity>> _logger;
        protected GenericRepository(DbContext context, ILogger<GenericRepository<TEntity>> logger)
        {
            _entities = context.Set<TEntity>();
            _dbContext = context;
            _logger = logger;
        }

        public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<TEntity>> AllAsync(CancellationToken cancellationToken)
        {
            return await _entities.AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }
        
        public TEntity Update(TEntity entity, CancellationToken cancellationToken)
        {
            var entityEntry = _entities.Update(entity);
            return entityEntry.Entity;
        }

        public async Task<bool> BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                _entities.UpdateRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkUpdate Error");
                throw new Exception("Hata : " + ex.Message);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.Where(predicate).CountAsync(cancellationToken);
        }
        
        public async Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
            => await query.FirstOrDefaultAsync(cancellationToken);

        public IQueryable<TEntity> Queryable(CancellationToken cancellationToken)
        => _entities;

        public IQueryable<TEntity> Include<TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> property, CancellationToken cancellationToken)
        => query.Include(property);

        public async Task BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                _entities.AddRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkAdd Error");
                throw new Exception("Hata : " + ex.Message);
            }

        }
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken)
            => query.ToListAsync(cancellationToken);

        public void Delete(TEntity entity, CancellationToken cancellationToken)
        {
            _entities.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entity, CancellationToken cancellationToken)
        {
            _entities.RemoveRange(entity);
        }

    }
}