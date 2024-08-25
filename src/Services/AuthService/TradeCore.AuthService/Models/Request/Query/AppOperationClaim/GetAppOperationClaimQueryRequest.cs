using TradeCore.AuthService.Models.Response.Query;

namespace TradeCore.AuthService.Models.Request.Query
{
    public class GetAppOperationClaimQueryRequest : RequestBase<GetAppOperationClaimQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
