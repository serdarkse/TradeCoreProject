using System.ComponentModel.DataAnnotations;
using TradeCore.OrderService.Domain.BaseEntity;
using TradeCore.OrderService.Domain.CustomerOrderAggregate;

namespace TradeCore.OrderService.Domain.ProductAggregate
{
    public class Product : Entity
    {
        [Required(ErrorMessage = "Barcode is required.")]
        [MaxLength(50, ErrorMessage = "Barcode can't be longer than 50 characters.")]
        public string Barcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public List<CustomerOrder> CustomerOrders { get; set; }

    }
}
