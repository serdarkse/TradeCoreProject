using TradeCore.AuthService.Models.Response.Command.AppCustomer;

namespace TradeCore.AuthService.Models.Request.Command.AppCustomer
{
    public class CreateAppCustomerCommandRequest : RequestBase<CreateAppCustomerCommandResponse>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
