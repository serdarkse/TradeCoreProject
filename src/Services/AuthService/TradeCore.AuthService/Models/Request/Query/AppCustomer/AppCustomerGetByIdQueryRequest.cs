using TradeCore.AuthService.Models.Response.Query.AppCustomer;

namespace TradeCore.AuthService.Models.Request.Query.AppCustomer
{
    public class AppCustomerGetByIdQueryRequest : RequestBase<AppCustomerGetByIdQueryResponse>
    {
        public Guid AppCustomerId { get; set; }
    }
}
