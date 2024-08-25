using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Response.Command.Product;

namespace TradeCore.OrderService.Models.Request.Command.Product
{
    public class AddProductCommandRequest : RequestBase<AddProductCommandResponse>
    {
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
