using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;

namespace TradeCore.OrderService.Utilities
{
    public static class QueryableExtensions
    {
        public static int CountWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTransaction())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                int toReturn = query.Count();
                scope.Complete();
                return toReturn;
            }
        }

        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            using (var scope = CreateTransactionAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                int toReturn = await query.CountAsync(cancellationToken);
                scope.Complete();
                return toReturn;
            }
        }

        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            using (var scope = CreateTransaction())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                T result = query.FirstOrDefault();
                scope.Complete();
                return result;
            }
        }

        public static async Task<T> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            using (var scope = CreateTransactionAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                T result = await query.FirstOrDefaultAsync(cancellationToken);
                scope.Complete();
                return result;
            }
        }

        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null)
        {
            List<T> result = default;
            using (var scope = CreateTransaction())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }

        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> expression = null, CancellationToken cancellationToken = default)
        {
            List<T> result = default;
            using (var scope = CreateTransactionAsync())
            {
                if (expression is object)
                {
                    query = query.Where(expression);
                }
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        private static TransactionScope CreateTransactionAsync()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        private static TransactionScope CreateTransaction()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
        }

        public static Expression<Func<T, bool>> CombineConditions<T>(List<Expression<Func<T, bool>>> conditions)
        {
            if (!conditions.Any())
            {
                return null;
            }

            var combinedCondition = conditions.Aggregate(AndAlso<T>);
            return combinedCondition;
        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
