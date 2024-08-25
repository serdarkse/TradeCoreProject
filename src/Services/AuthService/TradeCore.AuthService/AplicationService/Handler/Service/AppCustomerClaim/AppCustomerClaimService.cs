using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Repository;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public class AppCustomerClaimService : IAppCustomerClaimService
    {
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAppCustomerClaimService _appCustomerClaimService;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public AppCustomerClaimService(
            IDbContextHandler dbContextHandler,
            IAppCustomerClaimService appCustomerClaimService,
            IAppCustomerClaimRepository appCustomerClaimRepository,
            IAppOperationClaimRepository appOperationClaimRepository
            )
        {
            _dbContextHandler = dbContextHandler;
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appCustomerClaimService = appCustomerClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public async Task<AppCustomerClaim> CreateappCustomerClaim(CreateAppCustomerClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var userClaim = new AppCustomerClaim
            {
                AppOperationClaimId = request.AppOperationClaimId,
                AppCustomerId = request.AppCustomerId
            };

            await _appCustomerClaimRepository.SaveAsync(userClaim, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return userClaim;
        }

        public async Task<AppCustomerClaim> DeleteappCustomerClaim(DeleteAppCustomerClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var entityToDelete = await _appCustomerClaimRepository.FindByAsync(x => x.Id == request.Id, cancellationToken);

            _appCustomerClaimRepository.Delete(entityToDelete, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return entityToDelete;
        }

        public async Task<AppCustomerClaim> UpdateappCustomerClaim(UpdateAppCustomerClaimCommandRequest request, AppCustomerClaim group, CancellationToken cancellationToken)
        {
            var userClaimRecord = await _appCustomerClaimRepository.FindByAsync(u => u.AppOperationClaimId == request.AppOperationClaimId && u.AppCustomerId == request.AppCustomerId, cancellationToken);

            userClaimRecord.AppOperationClaimId = request.AppOperationClaimId;

            _appCustomerClaimRepository.Update(userClaimRecord, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return userClaimRecord;
        }
    }
}
