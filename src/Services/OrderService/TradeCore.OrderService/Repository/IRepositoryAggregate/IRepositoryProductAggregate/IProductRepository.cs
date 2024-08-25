using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Models.Dtos;

namespace TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<ProductDto>> GetAllProducts(CancellationToken cancellationToken);
        Task<ProductDto> GetProduct(Guid id, CancellationToken cancellationToken);

    }
}
