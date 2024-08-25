namespace TradeCore.OrderService.Repository
{
    public interface IDbContextHandler
    {
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);

    }
}
