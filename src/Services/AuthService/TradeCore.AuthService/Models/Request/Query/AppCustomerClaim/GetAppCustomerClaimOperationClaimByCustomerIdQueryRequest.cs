using TradeCore.AuthService.Models.Response.Query;

namespace TradeCore.AuthService.Models.Request.Query
{
    public class GetAppCustomerClaimOperationClaimByCustomerIdQueryRequest : RequestBase<GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
