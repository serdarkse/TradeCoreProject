using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.Repository.RepositoryAggregate.ReposiyoryaApCustomerClaimAggregate
{
    public class AppCustomerClaimRepository : GenericRepository<AppCustomerClaim>, IAppCustomerClaimRepository
    {
        public AppCustomerClaimRepository(AuthDbContext context, ILogger<AppCustomerClaimRepository> logger) : base(context, logger)
        {

        }
    }
}
