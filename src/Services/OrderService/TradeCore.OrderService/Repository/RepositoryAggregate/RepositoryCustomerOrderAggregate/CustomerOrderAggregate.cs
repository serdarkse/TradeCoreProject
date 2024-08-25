using Microsoft.EntityFrameworkCore;
using TradeCore.OrderService.Domain.CustomerOrderAggregate;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;
using TradeCore.OrderService.Utilities;

namespace TradeCore.OrderService.Repository.RepositoryAggregate.RepositoryCustomerOrderAggregate
{
    public class CustomerOrderAggregate : GenericRepository<CustomerOrder>, ICustomerOrderRepository
    {
        private readonly IDbContextFactory _dbContextFactory;

        public CustomerOrderAggregate(OrderDbContext context, ILogger<CustomerOrderAggregate> logger, IDbContextFactory dbContextFactory) : base(context, logger)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<CustomerOrderDto>> GetAllOrders(CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking()
                .Select(x => new CustomerOrderDto
                {
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    CustomerAddress = x.CustomerAddress,
                    Products = x.Products.Select(a=>new ProductDto { Barcode = a.Barcode, Description=a.Description, Id=a.Id, Price=a.Price, Quantity=a.Quantity}).ToList()

                }).ToListWithNoLockAsync();
        }

        public async Task<CustomerOrderDto> GetCustomerOrder(Guid id, CancellationToken cancellationToken)
        {
            var customerOrder = await _entities.AsNoTracking()
                                      .Where(p => p.Id == id)
                                      .Select(l => new CustomerOrderDto
                                      {
                                          CustomerId = l.CustomerId,
                                          CustomerName = l.CustomerName,
                                          CustomerAddress = l.CustomerAddress,
                                          Products = l.Products.Select(a => new ProductDto { Barcode = a.Barcode, Description = a.Description, Id = a.Id, Price = a.Price, Quantity = a.Quantity }).ToList()

                                      }).FirstOrDefaultWithNoLockAsync(cancellationToken: cancellationToken);

            return customerOrder;
        }
    }
}