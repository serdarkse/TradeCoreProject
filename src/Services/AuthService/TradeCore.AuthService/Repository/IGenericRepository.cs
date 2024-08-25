using System.Linq.Expressions;
using TradeCore.AuthService.Domain.BaseEntity;

namespace TradeCore.AuthService.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<TEntity>> AllAsync(CancellationToken cancellationToken);
        Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<List<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
        TEntity Update(TEntity entity, CancellationToken cancellationToken);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        IQueryable<TEntity> Include<TProperty>(IQueryable<TEntity> query, Expression<Func<TEntity, TProperty>> property, CancellationToken cancellationToken);
        IQueryable<TEntity> Queryable(CancellationToken cancellationToken);
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);
        Task BulkAddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken);
        Task<bool> BulkUpdateAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void Delete(TEntity entity, CancellationToken cancellationToken);
        void DeleteRange(IEnumerable<TEntity> entity, CancellationToken cancellationToken);
    }
}
