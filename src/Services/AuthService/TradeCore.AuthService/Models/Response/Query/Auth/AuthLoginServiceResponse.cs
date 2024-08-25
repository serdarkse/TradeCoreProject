using TradeCore.AuthService.Models.Dtos;

namespace TradeCore.AuthService.Models.Response.Query.Auth
{
    public class AuthLoginServiceResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public AppCustomerDto CustomerInfo { get; set; }
    }
}
