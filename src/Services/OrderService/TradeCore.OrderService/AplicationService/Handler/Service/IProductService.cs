using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Request.Command.Product;

namespace TradeCore.OrderService.AplicationService.Handler.Service
{
    public interface IProductService
    {
        Task<ProductDto> AddProduct(AddProductCommandRequest request, CancellationToken cancellationToken);
        Task<ProductDto> UpdateProduct(UpdateProductCommandRequest request, Product product, CancellationToken cancellationToken);
        Task<bool> DeleteProduct(DeleteProductCommandRequest request, CancellationToken cancellationToken);
    }
}
