using TradeCore.AuthService.Helpers.HelperEnums;

namespace TradeCore.AuthService.Helpers.HelperModels
{
    public class AppToken : AccessToken
    {
        public AuthenticationProviderType Provider { get; set; }
    }
}
