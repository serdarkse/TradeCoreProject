using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class UpdateAppCustomerClaimCommandRequest : RequestBase<UpdateAppCustomerClaimCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid AppCustomerId { get; set; }
        public Guid AppOperationClaimId { get; set; }
    }
}
