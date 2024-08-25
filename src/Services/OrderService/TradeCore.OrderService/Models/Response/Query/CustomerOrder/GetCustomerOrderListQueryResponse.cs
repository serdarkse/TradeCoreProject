using TradeCore.OrderService.Models.Dtos;

namespace TradeCore.OrderService.Models.Response.Query.CustomerOrder
{
    public class GetCustomerOrderListQueryResponse
    {
        public List<CustomerOrderDto> CustomerOrderList { get; set; }
    }
}
