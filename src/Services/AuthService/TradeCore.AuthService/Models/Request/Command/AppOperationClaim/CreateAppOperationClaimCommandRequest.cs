
using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class CreateAppOperationClaimCommandRequest : RequestBase<CreateAppOperationClaimCommandResponse>
    {
        public string FunctionName { get; set; }
        public Guid ParentFunctionId { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
    }
}
