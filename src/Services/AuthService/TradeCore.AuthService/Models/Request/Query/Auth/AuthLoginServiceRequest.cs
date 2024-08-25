using TradeCore.AuthService.Models.Response.Query.Auth;

namespace TradeCore.AuthService.Models.Request.Query.Auth
{
    public class AuthLoginServiceRequest : RequestBase<AuthLoginServiceResponse>
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
