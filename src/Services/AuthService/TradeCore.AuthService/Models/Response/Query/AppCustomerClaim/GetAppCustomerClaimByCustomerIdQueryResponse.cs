using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;

namespace TradeCore.AuthService.Models.Response.Query
{
    public class GetAppCustomerClaimByCustomerIdQueryResponse
    {
        public List<AppCustomerClaim> Data { get; set; }

    }
}
