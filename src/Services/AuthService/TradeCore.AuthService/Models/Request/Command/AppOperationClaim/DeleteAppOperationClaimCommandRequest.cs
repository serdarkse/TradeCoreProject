using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class DeleteAppOperationClaimCommandRequest : RequestBase<DeleteAppOperationClaimCommandResponse>
    {
        public Guid Id { get; set; }

    }
}
