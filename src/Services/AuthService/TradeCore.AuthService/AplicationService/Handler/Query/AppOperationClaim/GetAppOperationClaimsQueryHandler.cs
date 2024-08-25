using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query
{
    public class GetAppOperationClaimsQueryHandler : RequestHandlerBase<GetAppOperationClaimsQueryRequest, GetAppOperationClaimsQueryResponse>
    {
        private readonly IAppOperationClaimService _appOperationClaimService;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public GetAppOperationClaimsQueryHandler(IAppOperationClaimRepository appOperationClaimRepository, IAppOperationClaimService appOperationClaimService)
        {
            _appOperationClaimService = appOperationClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<GetAppOperationClaimsQueryResponse>> Handle(GetAppOperationClaimsQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appOperationClaimRepository.AllAsync(cancellationToken);
            if (!list.Any())
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());


            List<TreeView> listModel = null;

            return new ResponseBase<GetAppOperationClaimsQueryResponse> { Data = new GetAppOperationClaimsQueryResponse { Data = listModel }, Success = true };
        }
    }
}