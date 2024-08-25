using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;

namespace TradeCore.OrderService.Models.Request.Command.CustomerOrder
{
    public class DeleteCustomerOrderCommandRequest : RequestBase<DeleteCustomerOrderCommandResponse>
    {
        public Guid CustomerId { get; set; }
    }
}
