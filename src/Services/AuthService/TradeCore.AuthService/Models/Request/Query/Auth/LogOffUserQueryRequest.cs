using TradeCore.AuthService.Models.Response.Query.Auth;

namespace TradeCore.AuthService.Models.Request.Query.Auth
{
    public class LogOffUserQueryRequest : RequestBase<LogOffUserQueryResponse>
    {
        public string Name { get; set; }
    }
}
