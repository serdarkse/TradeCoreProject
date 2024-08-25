namespace TradeCore.OrderService.Repository
{
    public interface IDbContextFactory
    {
        OrderDbContext CreateDbContext();
    }
}
