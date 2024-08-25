using TradeCore.AuthService.Domain.AppCustomerAggregate;

namespace TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate
{
    public interface ITokenHelper 
    {
        TAccessToken CreateToken<TAccessToken>(AppCustomer user) where TAccessToken : IAccessToken, new();
    }
}
