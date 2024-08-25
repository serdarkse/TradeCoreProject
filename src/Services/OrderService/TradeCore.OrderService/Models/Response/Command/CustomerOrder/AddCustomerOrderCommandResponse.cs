using TradeCore.OrderService.Models.Dtos;

namespace TradeCore.OrderService.Models.Response.Command.CustomerOrder
{
    public class AddCustomerOrderCommandResponse
    {
        public Guid CustomerId { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
