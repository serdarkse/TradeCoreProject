using TradeCore.AuthService.Models.Response.Query;

namespace TradeCore.AuthService.Models.Request.Query
{
    public class GetAppCustomerClaimByUserIdQueryRequest : RequestBase<GetAppCustomerClaimByCustomerIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
