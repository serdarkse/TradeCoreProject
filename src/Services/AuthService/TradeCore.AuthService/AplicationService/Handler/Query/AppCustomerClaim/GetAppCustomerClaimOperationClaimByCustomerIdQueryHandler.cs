using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query
{
    public class GetAppCustomerClaimOperationClaimByCustomerIdQueryHandler : RequestHandlerBase<GetAppCustomerClaimOperationClaimByCustomerIdQueryRequest, GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse>
    {
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        public GetAppCustomerClaimOperationClaimByCustomerIdQueryHandler(IAppCustomerClaimRepository appCustomerClaimRepository)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
        }

        public override async Task<ResponseBase<GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse>> Handle(GetAppCustomerClaimOperationClaimByCustomerIdQueryRequest request, CancellationToken cancellationToken)
        {
            var list = new List<SelectionItem>();
            return new ResponseBase<GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse> { Data = new GetAppCustomerClaimOperationClaimByCustomerIdQueryResponse { Data = list }, Success = true };
        }
    }
}