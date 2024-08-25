using TradeCore.AuthService.Domain.AppOperationClaimAggregate;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Repository;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public class AppOperationClaimService : IAppOperationClaimService
    {
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public AppOperationClaimService(
            IDbContextHandler dbContextHandler,
            IAppOperationClaimRepository appOperationClaimRepository
            )
        {
            _dbContextHandler = dbContextHandler;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public async Task<AppOperationClaim> CreateAppOperationClaim(CreateAppOperationClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var operationClaim = new AppOperationClaim
            {
                FunctionName = request.FunctionName,
                Alias = request.Alias,
                Description = request.Description,
                ParentFunctionId = request.ParentFunctionId,
            };

            await _appOperationClaimRepository.SaveAsync(operationClaim, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return operationClaim;
        }

        public async Task<AppOperationClaim> DeleteAppOperationClaim(DeleteAppOperationClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var claimToDelete = await _appOperationClaimRepository.FindByAsync(x => x.Id == request.Id, cancellationToken);
            _appOperationClaimRepository.Delete(claimToDelete, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return claimToDelete;
        }

        public async Task<AppOperationClaim> UpdateAppOperationClaim(UpdateAppOperationClaimCommandRequest request, AppOperationClaim group, CancellationToken cancellationToken)
        {
            var isOperationClaimsExits = await _appOperationClaimRepository.FindByAsync(u => u.Id == request.Id, cancellationToken);
            isOperationClaimsExits.Alias = request.Alias;
            isOperationClaimsExits.Description = request.Description;
            isOperationClaimsExits.ParentFunctionId = request.ParentFunctionId;
            isOperationClaimsExits.FunctionName = request.FunctionName;

            _appOperationClaimRepository.Update(isOperationClaimsExits, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return isOperationClaimsExits;
        }

    }
}
