using TradeCore.AuthService.Models.Response.Command.AppCustomer;

namespace TradeCore.AuthService.Models.Request.Command.AppCustomer
{
    public class DeleteAppCustomerCommandRequest : RequestBase<DeleteAppCustomerCommandResponse>
    {
        public Guid CustomerId { get; set; }

    }
}
