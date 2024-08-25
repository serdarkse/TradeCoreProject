using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Models.Dtos;

namespace TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate
{
    public interface IAppCustomerRepository : IGenericRepository<AppCustomer>
    {
        Task<List<AppCustomerDto>> GetAllUserList(CancellationToken cancellationToken);
        Task<AppCustomerDto> GetUserById(Guid AppCustomerId, CancellationToken cancellationToken);
        Task<bool> GetUserByEmail(string email, CancellationToken cancellationToken);

    }
}
