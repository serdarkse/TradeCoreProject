using TradeCore.OrderService.Domain.CustomerOrderAggregate;
using TradeCore.OrderService.Models.Dtos;

namespace TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate
{
    public interface ICustomerOrderRepository : IGenericRepository<CustomerOrder>
    {
        Task<List<CustomerOrderDto>> GetAllOrders(CancellationToken cancellationToken);
        Task<CustomerOrderDto> GetCustomerOrder(Guid id, CancellationToken cancellationToken);

    }
}
