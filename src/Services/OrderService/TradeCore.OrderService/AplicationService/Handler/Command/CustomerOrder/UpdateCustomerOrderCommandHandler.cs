using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.CustomerOrder
{
    public class UpdateCustomerOrderCommandHandler : RequestHandlerBase<UpdateCustomerOrderCommandRequest, UpdateCustomerOrderCommandResponse>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly ICustomerOrderService _customerOrderService;
        public UpdateCustomerOrderCommandHandler(ICustomerOrderRepository customerOrderRepository, ICustomerOrderService customerOrderService)
        {
            _customerOrderRepository = customerOrderRepository;
            _customerOrderService = customerOrderService;
        }

        public override async Task<ResponseBase<UpdateCustomerOrderCommandResponse>> Handle(UpdateCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var productForCustomer = await _customerOrderRepository.FindByAsync(p => p.CustomerId == request.CustomerId, cancellationToken);

            if (productForCustomer == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                                ApplicationMessage.NotExistCustomerId.Message(),
                                                ApplicationMessage.NotExistCustomerId.UserMessage());

            var addProductForCustomer = await _customerOrderService.UpdateCustomerOrder(request, productForCustomer, cancellationToken);

            return new ResponseBase<UpdateCustomerOrderCommandResponse> { Data = new UpdateCustomerOrderCommandResponse { }, Success = true, UserMessage = "Başarılı bir şekilde silindi." };
        }
    }
}
