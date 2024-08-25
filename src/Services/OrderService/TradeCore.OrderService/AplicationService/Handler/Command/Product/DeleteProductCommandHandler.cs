using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.Product;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.Product
{
    public class DeleteProductCommandHandler : RequestHandlerBase<DeleteProductCommandRequest, DeleteProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        public DeleteProductCommandHandler(IProductRepository productRepository, IProductService productService)
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        public override async Task<ResponseBase<DeleteProductCommandResponse>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FilterByAsync(p => p.Id == request.Id, cancellationToken);

            if (product.Any())
                throw new BusinessRuleException(ApplicationMessage.NotExistProduct.Code(),
                                                ApplicationMessage.NotExistProduct.Message(),
                                                ApplicationMessage.NotExistProduct.UserMessage());

            var addProduct = await _productService.DeleteProduct(request, cancellationToken);

            return new ResponseBase<DeleteProductCommandResponse> { Data = new DeleteProductCommandResponse { }, Success = true, UserMessage = "Başarılı bir şekilde silindi." };
        }
    }
}
