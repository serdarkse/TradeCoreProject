using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;

namespace TradeCore.AuthService.Models.Response.Query
{
    public class GetAppCustomerClaimsQueryResponse
    {
        public List<AppCustomerClaim> Data { get; set; }
    }
}
