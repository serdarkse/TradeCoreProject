using TradeCore.OrderService.Domain.CustomerOrderAggregate;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;

namespace TradeCore.OrderService.AplicationService.Handler.Service
{
    public interface ICustomerOrderService
    {
        Task<CustomerOrderDto> AddCustomerOrder(AddCustomerOrderCommandRequest request, CancellationToken cancellationToken);
        Task<CustomerOrderDto> UpdateCustomerOrder(UpdateCustomerOrderCommandRequest request, CustomerOrder customerOrder, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerOrder(DeleteCustomerOrderCommandRequest request, CancellationToken cancellationToken);
    }
}
