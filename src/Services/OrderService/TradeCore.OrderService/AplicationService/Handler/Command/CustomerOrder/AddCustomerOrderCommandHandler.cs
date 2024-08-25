using TradeCore.OrderService.AplicationService.Handler.Service;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;
using TradeCore.OrderService.Models.Request.Command.Product;
using TradeCore.OrderService.Models.Response;
using TradeCore.OrderService.Models.Response.Command.CustomerOrder;
using TradeCore.OrderService.Models.Response.Command.Product;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Command.CustomerOrder
{
    public class AddCustomerOrderCommandHandler : RequestHandlerBase<AddCustomerOrderCommandRequest, AddCustomerOrderCommandResponse>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly ICustomerOrderService _customerOrderService;
        public AddCustomerOrderCommandHandler(ICustomerOrderRepository customerOrderRepository, ICustomerOrderService customerOrderService)
        {
            _customerOrderRepository = customerOrderRepository;
            _customerOrderService = customerOrderService;
        }

        public override async Task<ResponseBase<AddCustomerOrderCommandResponse>> Handle(AddCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var productForCustomer = await _customerOrderRepository.FilterByAsync(p => p.CustomerId == request.CustomerId, cancellationToken);

            if (productForCustomer.Any())
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                                ApplicationMessage.NotExistCustomerId.Message(),
                                                ApplicationMessage.NotExistCustomerId.UserMessage());

            var addProductForCustomer = await _customerOrderService.AddCustomerOrder(request, cancellationToken);

            return new ResponseBase<AddCustomerOrderCommandResponse> { Data = new AddCustomerOrderCommandResponse { CustomerId = addProductForCustomer.CustomerId, Products = addProductForCustomer.Products }, Success = true, UserMessage = "Başarılı bir şekilde kaydedildi." };
        }
    }
}
