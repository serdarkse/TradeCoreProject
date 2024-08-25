using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query
{
    public class GetAppCustomerClaimByUserIdQueryHandler : RequestHandlerBase<GetAppCustomerClaimByUserIdQueryRequest, GetAppCustomerClaimByCustomerIdQueryResponse>
    {
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        public GetAppCustomerClaimByUserIdQueryHandler(IAppCustomerClaimRepository appCustomerClaimRepository)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
        }

        public override async Task<ResponseBase<GetAppCustomerClaimByCustomerIdQueryResponse>> Handle(GetAppCustomerClaimByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appCustomerClaimRepository.FilterByAsync(a => a.AppCustomerId == request.Id, cancellationToken);
            if (list == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<GetAppCustomerClaimByCustomerIdQueryResponse> { Data = new GetAppCustomerClaimByCustomerIdQueryResponse { Data = list }, Success = true };
        }
    }
}