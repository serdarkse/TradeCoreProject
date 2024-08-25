namespace TradeCore.AuthService.Repository
{
    public class DbContextHandler : IDbContextHandler
    {
        private readonly AuthDbContext _dbContext;
        public DbContextHandler(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
