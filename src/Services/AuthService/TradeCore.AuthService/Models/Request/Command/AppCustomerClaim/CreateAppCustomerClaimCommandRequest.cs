using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class CreateAppCustomerClaimCommandRequest : RequestBase<CreateAppCustomerClaimCommandResponse>
    {
        public Guid AppCustomerId { get; set; }
        public Guid AppOperationClaimId { get; set; }
    }
}
