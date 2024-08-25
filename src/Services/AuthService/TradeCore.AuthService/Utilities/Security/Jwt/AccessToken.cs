namespace TradeCore.AuthService.Security.Jwt
{
    public class AccessToken : IAccessToken
    {
        public List<string> Claims { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
