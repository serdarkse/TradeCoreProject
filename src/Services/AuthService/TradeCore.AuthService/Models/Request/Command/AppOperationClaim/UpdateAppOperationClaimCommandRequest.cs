using TradeCore.AuthService.Models.Response.Command;

namespace TradeCore.AuthService.Models.Request.Command
{
    public class UpdateAppOperationClaimCommandRequest : RequestBase<UpdateAppOperationClaimCommandResponse>
    {
        public Guid Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public Guid ParentFunctionId { get; set; }
        public string FunctionName { get; set; }
    }
}
