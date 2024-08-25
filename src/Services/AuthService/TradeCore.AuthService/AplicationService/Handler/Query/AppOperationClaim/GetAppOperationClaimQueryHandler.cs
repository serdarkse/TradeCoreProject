using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Query;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Query
{
    public class GetAppOperationClaimQueryHandler : RequestHandlerBase<GetAppOperationClaimQueryRequest, GetAppOperationClaimQueryResponse>
    {
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public GetAppOperationClaimQueryHandler(IAppOperationClaimRepository appOperationClaimRepository)
        {
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<GetAppOperationClaimQueryResponse>> Handle(GetAppOperationClaimQueryRequest request, CancellationToken cancellationToken)
        {
            var list = await _appOperationClaimRepository.GetByIdAsync(request.Id, cancellationToken);
            if (list == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(),
                                                ApplicationMessage.InvalidId.Message(),
                                                ApplicationMessage.InvalidId.UserMessage());

            return new ResponseBase<GetAppOperationClaimQueryResponse> { Data = new GetAppOperationClaimQueryResponse { Data = list }, Success = true };
        }
    }
}