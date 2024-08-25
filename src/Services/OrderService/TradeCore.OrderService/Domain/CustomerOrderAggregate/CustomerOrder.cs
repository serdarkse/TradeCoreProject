using TradeCore.OrderService.Domain.BaseEntity;
using TradeCore.OrderService.Domain.ProductAggregate;

namespace TradeCore.OrderService.Domain.CustomerOrderAggregate
{
    public class CustomerOrder : Entity
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
