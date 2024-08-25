using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.Product;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.Product
{
    public class AddProductCommandHandler : RequestHandlerBase<AddProductCommandRequest, AddProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        public AddProductCommandHandler(IProductRepository productRepository, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        public override async Task<ResponseBase<AddProductCommandResponse>> Handle(AddProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FilterByAsync(p => p.Barcode == request.Barcode, cancellationToken);

            if (product.Any())
                throw new BusinessRuleException(ApplicationMessage.ExistProduct.Code(),
                                                ApplicationMessage.ExistProduct.Message(),
                                                ApplicationMessage.ExistProduct.UserMessage());

            var addProduct = await _productService.AddProduct(request, cancellationToken);

            return new ResponseBase<AddProductCommandResponse> { Data = new AddProductCommandResponse { Product = addProduct }, Success = true, UserMessage = "Başarılı bir şekilde kaydedildi." };
        }
    }
}
