using TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate;

namespace TradeCore.AuthService.Helpers.HelperModels
{
    public class AccessToken : IAccessToken
    {
        public List<string> Claims { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
