using TradeCore.AuthService.Domain.AppOperationClaimAggregate;
using TradeCore.AuthService.Models.Request.Command;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public interface IAppOperationClaimService
    {
        Task<AppOperationClaim> CreateAppOperationClaim(CreateAppOperationClaimCommandRequest request, CancellationToken cancellationToken);
        Task<AppOperationClaim> UpdateAppOperationClaim(UpdateAppOperationClaimCommandRequest request, AppOperationClaim group, CancellationToken cancellationToken);
        Task<AppOperationClaim> DeleteAppOperationClaim(DeleteAppOperationClaimCommandRequest request, CancellationToken cancellationToken);
    }
}
