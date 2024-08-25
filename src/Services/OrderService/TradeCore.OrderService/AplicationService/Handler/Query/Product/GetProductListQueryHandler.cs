using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Query.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Query.CustomerOrder;
using TradeCore.OrderService.Models.Response.Query.Product;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Query.Product
{
    public class GetProductListQueryHandler : RequestHandlerBase<GetProductListQueryRequest, GetProductListQueryResponse>
    {
        private readonly IProductRepository _productRepository;
        public GetProductListQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<ResponseBase<GetProductListQueryResponse>> Handle(GetProductListQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _productRepository.GetAllProducts(cancellationToken);

            if (!list.Any())
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<GetProductListQueryResponse> { Data = new GetProductListQueryResponse { Products = list }, Success = true };
        }
    }
}