using TradeCore.OrderService.Domain.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TradeCore.OrderService.Repository
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

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _entities.SingleOrDefaultAsync(s => s.Id == id);
        }


        public async Task<List<TEntity>> AllAsync()
        {
            return await _entities.AsQueryable().ToListAsync();
        }

        public async Task<List<TEntity>> AllAsNoTrackingAsync()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity> FirstByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }


        public async Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.Where(predicate).ToListAsync(cancellationToken);
        }


        public async Task SaveAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }


        public TEntity Update(TEntity entity)
        {
            var entityEntry = _entities.Update(entity);
            return entityEntry.Entity;
        }

        public async Task<bool> BulkUpdateAsync(List<TEntity> entities)
        {
            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _entities.UpdateRange(entities);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkUpdateAsync Error");
                throw new Exception("Hata : " + ex.Message);
            }
        }

        public async Task<bool> BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            try
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                _entities.UpdateRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkUpdateAsync Error");
                throw new Exception("Error: " + ex.Message);
            }
        }


        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _entities.Where(predicate).CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _entities.Where(predicate).CountAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
            => query.FirstOrDefaultAsync();


        public IQueryable<TEntity> Queryable()
        => _entities;


        public IQueryable<TEntity> Include<TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> property)
        => query.Include(property);

        public async Task BulkAddAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _entities.AddRange(entities);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BulkAddAsync Error");
                throw new Exception("Hata : " + ex.Message);
            }

        }

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
                _logger.LogError(ex, "BulkAddAsync Error");
                throw new Exception("Error: " + ex.Message);
            }
        }

        public async Task BulkAddWithOutTransactionAsync(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }


        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
            => query.ToListAsync();


        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }


        public void DeleteRange(IEnumerable<TEntity> entity)
        {
            _entities.Where(x => entity.Contains(x)).ExecuteDelete();
        }
    }
}