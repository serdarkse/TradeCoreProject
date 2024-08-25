namespace TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate
{
    public interface IAccessToken
    {
        DateTime Expiration { get; set; }
        string Token { get; set; }
    }
}