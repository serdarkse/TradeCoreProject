using Microsoft.EntityFrameworkCore;
using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;
using TradeCore.OrderService.Utilities;

namespace TradeCore.OrderService.Repository.RepositoryAggregate.RepositoryProductAggregate
{
    public class ProductAggregate : GenericRepository<Product>, IProductRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public ProductAggregate(OrderDbContext context, ILogger<ProductAggregate> logger, IDbContextFactory dbContextFactory) : base(context, logger)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<ProductDto>> GetAllProducts(CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking()
               .Select(x => new ProductDto
               {
                   Id = x.Id,
                   Barcode = x.Barcode,
                   Description = x.Description,
                   Price = x.Price,
                   Quantity = x.Quantity

               }).ToListWithNoLockAsync(cancellationToken: cancellationToken);
        }

        public async Task<ProductDto> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            var product = await _entities.AsNoTracking()
                            .Where(p => p.Id == id)
                            .Select(l => new ProductDto
                            {
                                Id = l.Id,
                                Price=l.Price,
                                Quantity = l.Quantity,
                                Barcode=l.Barcode,
                                Description = l.Description

                            }).FirstOrDefaultWithNoLockAsync(cancellationToken: cancellationToken);

            return product;
        }
    }
}
