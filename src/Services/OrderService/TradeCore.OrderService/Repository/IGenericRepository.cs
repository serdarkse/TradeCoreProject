using TradeCore.OrderService.Domain.BaseEntity;
using System.Linq.Expressions;

namespace TradeCore.OrderService.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<List<TEntity>> AllAsync();
        Task<List<TEntity>> AllAsNoTrackingAsync();
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> FirstByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task SaveAsync(TEntity entity);
        Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
        //to do update has to be async
        TEntity Update(TEntity entity);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        IQueryable<TEntity> Include<TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> property);
        IQueryable<TEntity> Queryable();
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query);
        Task BulkAddAsync(IEnumerable<TEntity> entities);
        Task BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task BulkAddWithOutTransactionAsync(IEnumerable<TEntity> entities);
        Task<List<T>> ToListAsync<T>(IQueryable<T> query);
        Task<bool> BulkUpdateAsync(List<TEntity> entities);
        Task<bool> BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entity);
    }
}
