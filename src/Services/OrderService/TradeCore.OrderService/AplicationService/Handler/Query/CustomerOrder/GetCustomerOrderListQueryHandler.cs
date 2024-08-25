using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Query.CustomerOrder;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Query.CustomerOrder;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Query.CustomerOrder
{
    public class GetCustomerOrderListQueryHandler : RequestHandlerBase<GetCustomerOrderListQueryRequest, GetCustomerOrderListQueryResponse>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        public GetCustomerOrderListQueryHandler(ICustomerOrderRepository customerOrderRepository)
        {
            _customerOrderRepository = customerOrderRepository;
        }

        public override async Task<ResponseBase<GetCustomerOrderListQueryResponse>> Handle(GetCustomerOrderListQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _customerOrderRepository.GetAllOrders(cancellationToken);

            if (!list.Any())
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<GetCustomerOrderListQueryResponse> { Data = new GetCustomerOrderListQueryResponse {  CustomerOrderList = list }, Success = true };
        }
    }
}