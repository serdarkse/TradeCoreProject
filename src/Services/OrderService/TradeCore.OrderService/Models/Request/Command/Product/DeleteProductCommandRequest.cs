using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Response.Command.Product;

namespace TradeCore.OrderService.Models.Request.Command.Product
{
    public class DeleteProductCommandRequest : RequestBase<DeleteProductCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
