using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query
{
    public class GetAppCustomerClaimsQueryHandler : RequestHandlerBase<GetAppCustomerClaimsQueryRequest, GetAppCustomerClaimsQueryResponse>
    {
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        public GetAppCustomerClaimsQueryHandler(IAppCustomerClaimRepository appCustomerClaimRepository)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
        }

        public override async Task<ResponseBase<GetAppCustomerClaimsQueryResponse>> Handle(GetAppCustomerClaimsQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appCustomerClaimRepository.AllAsync(cancellationToken);
            if (!list.Any())
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<GetAppCustomerClaimsQueryResponse> { Data = new GetAppCustomerClaimsQueryResponse { Data = list }, Success = true };
        }
    }
}