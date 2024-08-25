using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class DeleteAppCustomerClaimCommandRequest : RequestBase<DeleteAppCustomerClaimCommandResponse>
    {
        public Guid Id { get; set; }

    }
}
