using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query.AppCustomer
{
    public class AppCustomerGetAllListQueryHandler : RequestHandlerBase<AppCustomerGetAllListQueryRequest, AppCustomerGetAllListQueryResponse>
    {
        private readonly IAppCustomerRepository _appCustomerRepository;
        public AppCustomerGetAllListQueryHandler(IAppCustomerRepository appCustomerRepository)
        {
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<AppCustomerGetAllListQueryResponse>> Handle(AppCustomerGetAllListQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appCustomerRepository.GetAllUserList(cancellationToken);
            if (!list.Any())
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<AppCustomerGetAllListQueryResponse> { Data = new AppCustomerGetAllListQueryResponse { Data = list }, Success = true };
        }
    }
}