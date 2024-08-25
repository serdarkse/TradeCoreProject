using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query.AppCustomer
{
    public class AppCustomerGetByIdQueryHandler : RequestHandlerBase<AppCustomerGetByIdQueryRequest, AppCustomerGetByIdQueryResponse>
    {
        private readonly IAppCustomerRepository _appCustomerRepository;
        public AppCustomerGetByIdQueryHandler(IAppCustomerRepository appCustomerRepository)
        {
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<AppCustomerGetByIdQueryResponse>> Handle(AppCustomerGetByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appCustomerRepository.GetUserById(request.AppCustomerId, cancellationToken);
            if (list == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<AppCustomerGetByIdQueryResponse> { Data = new AppCustomerGetByIdQueryResponse { AppCustomer = list }, Success = true };
        }
    }
}