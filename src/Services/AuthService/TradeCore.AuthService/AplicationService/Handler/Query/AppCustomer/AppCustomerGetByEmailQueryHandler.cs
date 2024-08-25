using TradeCore.AuthService.Models.Request.Query.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query.AppCustomer
{
    public class AppCustomerGetByEmailQueryHandler : RequestHandlerBase<AppCustomerGetByEmailQueryRequest, AppCustomerGetByEmailQueryResponse>
    {
        private readonly IAppCustomerRepository _appCustomerRepository;
        public AppCustomerGetByEmailQueryHandler(IAppCustomerRepository appCustomerRepository)
        {
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<AppCustomerGetByEmailQueryResponse>> Handle(AppCustomerGetByEmailQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appCustomerRepository.GetUserByEmail(request.EmailAddress, cancellationToken);
           
            return new ResponseBase<AppCustomerGetByEmailQueryResponse> { Success = list };
        }
    }
}