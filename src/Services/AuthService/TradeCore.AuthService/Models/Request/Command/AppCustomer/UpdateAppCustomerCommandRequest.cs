using TradeCore.AuthService.Models.Response.Command.AppCustomer;

namespace TradeCore.AuthService.Models.Request.Command.AppCustomer
{
    public class UpdateAppCustomerCommandRequest : RequestBase<UpdateAppCustomerCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public bool IsBlocked { get; set; }
        public string InputType { get; set; }
        public bool IsSystemAdmin { get; set; }
        public List<Guid> AuthList { get; set; }
    }
}
