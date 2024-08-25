using TradeCore.AuthService.Domain.BaseEntity;

namespace TradeCore.AuthService.Domain.AppCustomerClaimAggregate
{
    public partial class AppCustomerClaim : Entity
    {
        public Guid AppCustomerId { get; set; }
        public Guid AppOperationClaimId { get; set; }
    }
}
