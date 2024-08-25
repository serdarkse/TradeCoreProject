using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;

namespace TradeCore.OrderService.Models.Request.Command.CustomerOrder
{
    public class AddCustomerOrderCommandRequest : RequestBase<AddCustomerOrderCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public string CustomerAddress { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
