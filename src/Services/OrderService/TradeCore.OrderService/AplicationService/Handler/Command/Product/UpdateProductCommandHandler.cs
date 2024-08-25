using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.Product;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.Product
{
    public class UpdateProductCommandHandler : RequestHandlerBase<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        public UpdateProductCommandHandler(IProductRepository productRepository, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        public override async Task<ResponseBase<UpdateProductCommandResponse>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByAsync(p => p.Barcode == request.Barcode, cancellationToken);

            if (product == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistProduct.Code(),
                                                ApplicationMessage.NotExistProduct.Message(),
                                                ApplicationMessage.NotExistProduct.UserMessage());

            var addProduct = await _productService.UpdateProduct(request, product, cancellationToken);

            return new ResponseBase<UpdateProductCommandResponse> { Data = new UpdateProductCommandResponse { Product = addProduct }, Success = true, UserMessage = "Başarılı bir şekilde güncellendi." };
        }
    }
}
