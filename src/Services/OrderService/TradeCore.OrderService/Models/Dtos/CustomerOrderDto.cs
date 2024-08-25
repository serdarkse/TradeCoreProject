namespace TradeCore.OrderService.Models.Dtos
{
    public class CustomerOrderDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
