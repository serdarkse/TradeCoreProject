namespace TradeCore.OrderService.Repository
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly OrderDbContext _dbContext;
        public DbContextHandler(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
