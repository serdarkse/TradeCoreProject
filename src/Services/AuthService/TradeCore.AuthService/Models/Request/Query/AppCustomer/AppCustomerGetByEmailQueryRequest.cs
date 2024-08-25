using TradeCore.AuthService.Models.Response.Query.AppCustomer;

namespace TradeCore.AuthService.Models.Request.Query.AppCustomer
{
    public class AppCustomerGetByEmailQueryRequest : RequestBase<AppCustomerGetByEmailQueryResponse>
    {
        public string EmailAddress { get; set; }

    }
}
