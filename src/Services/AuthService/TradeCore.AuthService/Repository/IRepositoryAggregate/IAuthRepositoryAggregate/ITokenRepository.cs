using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Helpers.HelperModels;

namespace TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate
{
    public interface ITokenRepository : IGenericRepository<AppCustomer>
    {
        AppToken GenerateAccessToken(AppCustomer user);
        string GenerateRefreshToken();
    }
}
