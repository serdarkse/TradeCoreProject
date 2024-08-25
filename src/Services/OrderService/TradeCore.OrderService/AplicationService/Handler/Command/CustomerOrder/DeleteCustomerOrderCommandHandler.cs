using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.CustomerOrder
{
    public class DeleteCustomerOrderCommandHandler : RequestHandlerBase<DeleteCustomerOrderCommandRequest, DeleteCustomerOrderCommandResponse>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly ICustomerOrderService _customerOrderService;
        public DeleteCustomerOrderCommandHandler(ICustomerOrderRepository customerOrderRepository, ICustomerOrderService customerOrderService)
        {
            _customerOrderRepository = customerOrderRepository;
            _customerOrderService = customerOrderService;
        }

        public override async Task<ResponseBase<DeleteCustomerOrderCommandResponse>> Handle(DeleteCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var productForCustomer = await _customerOrderRepository.FilterByAsync(p => p.CustomerId == request.CustomerId, cancellationToken);

            if (productForCustomer.Any())
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                                ApplicationMessage.NotExistCustomerId.Message(),
                                                ApplicationMessage.NotExistCustomerId.UserMessage());

            var addProductForCustomer = await _customerOrderService.DeleteCustomerOrder(request, cancellationToken);

            return new ResponseBase<DeleteCustomerOrderCommandResponse> { Data = new DeleteCustomerOrderCommandResponse { }, Success = true, UserMessage = "Başarılı bir şekilde silindi." };
        }
    }
}
