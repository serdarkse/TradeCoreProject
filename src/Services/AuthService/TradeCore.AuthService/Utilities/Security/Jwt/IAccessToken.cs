namespace TradeCore.AuthService.Security.Jwt
{
    public interface IAccessToken
    {
        DateTime Expiration { get; set; }
        string Token { get; set; }
    }
}