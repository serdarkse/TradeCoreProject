using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.CrossCuttingConcerns.Caching;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Helpers;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Repository;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Service
{
    public class ProductService : IProductService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IConfiguration _configuration;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IProductRepository _productRepository;

        public ProductService(
            ICacheManager cacheManager,
            IConfiguration configuration,
            IDbContextHandler dbContextHandler,
            IProductRepository productRepository)
        {
            _cacheManager = cacheManager;
            _configuration = configuration;
            _dbContextHandler = dbContextHandler;
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddProduct(AddProductCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;
            var productList = new List<Product>();

            var getProductListFromCache = await _cacheManager.GetByPattern<Product>("ProductList");
            if (getProductListFromCache != null && getProductListFromCache.Count() > 0)
            {
                productList = getProductListFromCache.ToList();
            }
            else
            {
                var allProducts = await _productRepository.AllAsync();
                productList = allProducts;
                await _cacheManager.Add("ProductList",allProducts);
            }

            var barcodeControl = productList.Any(p => p.Barcode == request.Barcode);
            if (barcodeControl)
                throw new BusinessRuleException(ApplicationMessage.NotExistProduct.Code(),
                                             ApplicationMessage.NotExistProduct.Message(),
                                             ApplicationMessage.NotExistProduct.UserMessage());

            var addProduct = new Product
            {
                Id = Guid.NewGuid(),
                Price = request.Price,
                Barcode = request.Barcode,
                Quantity = request.Quantity,
                Description = request.Description,
                CreatedDate = DateTimeHelper.DateTimeUtcTimeZone(),
                IsActive = true,
                CreatedUserId = userId
            };

            await _productRepository.SaveAsync(addProduct, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            var addedProduct = await _productRepository.GetProduct(addProduct.Id, cancellationToken);

            return addedProduct;
        }

        public async Task<ProductDto> UpdateProduct(UpdateProductCommandRequest request, Product product, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;

            var productList = new List<Product>();

            var getProductListFromCache = await _cacheManager.GetByPattern<Product>("ProductList");
            if (getProductListFromCache != null && getProductListFromCache.Count() > 0)
            {
                productList = getProductListFromCache.ToList();
            }
            else
            {
                var allProducts = await _productRepository.AllAsync();
                productList = allProducts;
                await _cacheManager.Add("ProductList", allProducts);
            }

            var idControl = productList.FirstOrDefault(p => p.Id == request.Id);
            if (idControl == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistProduct.Code(),
                                             ApplicationMessage.NotExistProduct.Message(),
                                             ApplicationMessage.NotExistProduct.UserMessage());

            var productUpdate = new Product
            {
                Id = request.Id,
                Description = request.Description,
                Barcode = request.Barcode,
                Quantity = request.Quantity,
                Price = request.Price,
                ModifiedDate = DateTimeHelper.DateTimeUtcTimeZone(),
                CreatedUserId = userId
            };

            _productRepository.Update(productUpdate);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);


            var productDto = await _productRepository.GetProduct(product.Id, cancellationToken);

            return productDto;
        }

        public async Task<bool> DeleteProduct(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;

            var productList = new List<Product>();

            var getProductListFromCache = await _cacheManager.GetByPattern<Product>("ProductList");
            if (getProductListFromCache != null && getProductListFromCache.Count() > 0)
            {
                productList = getProductListFromCache.ToList();
            }
            else
            {
                var allProducts = await _productRepository.AllAsync();
                productList = allProducts;
                await _cacheManager.Add("ProductList", allProducts);
            }


            var idControl = productList.FirstOrDefault(p => p.Id == request.Id);
            if (idControl == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistProduct.Code(),
                                             ApplicationMessage.NotExistProduct.Message(),
                                             ApplicationMessage.NotExistProduct.UserMessage());


            _productRepository.Delete(idControl);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
