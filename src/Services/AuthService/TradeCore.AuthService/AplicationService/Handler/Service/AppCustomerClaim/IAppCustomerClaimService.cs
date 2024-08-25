using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Models.Request.Command;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public interface IAppCustomerClaimService
    {
        Task<AppCustomerClaim> CreateappCustomerClaim(CreateAppCustomerClaimCommandRequest request, CancellationToken cancellationToken);
        Task<AppCustomerClaim> UpdateappCustomerClaim(UpdateAppCustomerClaimCommandRequest request, AppCustomerClaim group, CancellationToken cancellationToken);
        Task<AppCustomerClaim> DeleteappCustomerClaim(DeleteAppCustomerClaimCommandRequest request, CancellationToken cancellationToken);
    }
}
