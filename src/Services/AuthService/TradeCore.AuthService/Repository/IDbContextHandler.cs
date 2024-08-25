namespace TradeCore.AuthService.Repository
{
    public interface IDbContextHandler
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
