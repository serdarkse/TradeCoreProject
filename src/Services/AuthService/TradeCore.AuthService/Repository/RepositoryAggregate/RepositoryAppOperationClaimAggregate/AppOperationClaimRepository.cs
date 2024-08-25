using TradeCore.AuthService.Domain.AppOperationClaimAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryAppOperationClaimAggregate
{
    public class AppOperationClaimRepository : GenericRepository<AppOperationClaim>, IAppOperationClaimRepository
    {
        public AppOperationClaimRepository(AuthDbContext context, ILogger<AppOperationClaimRepository> logger) : base(context, logger)
        {

        }
    }
}
