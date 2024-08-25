using Microsoft.EntityFrameworkCore;

namespace TradeCore.OrderService.Repository
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContextOptions<OrderDbContext> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbContextFactory(DbContextOptions<OrderDbContext> options)
        {
            _options = options;
        }

        public OrderDbContext CreateDbContext()
        {
            return new OrderDbContext(_options, _httpContextAccessor);
        }
    }
}