using TradeCore.AuthService.Domain.BaseEntity;

namespace TradeCore.AuthService.Domain.AppOperationClaimAggregate
{
    public partial class AppOperationClaim : Entity
    {
        public Guid ParentFunctionId { get; set; }
        public string FunctionName { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
    }
}
