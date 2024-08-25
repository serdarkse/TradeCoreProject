using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command
{
    public class UpdateAppOperationClaimCommandHandler : RequestHandlerBase<UpdateAppOperationClaimCommandRequest, UpdateAppOperationClaimCommandResponse>
    {
        private readonly IAppOperationClaimService _appOperationClaimService;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public UpdateAppOperationClaimCommandHandler(IAppOperationClaimRepository appOperationClaimRepository, IAppOperationClaimService appOperationClaimService)
        {
            _appOperationClaimService = appOperationClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<UpdateAppOperationClaimCommandResponse>> Handle(UpdateAppOperationClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appOperationClaimRepository.FindByAsync(p => p.FunctionName == request.FunctionName, cancellationToken);

            if (isUserAlreadyAdded == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            var createdUser = await _appOperationClaimService.UpdateAppOperationClaim(request, isUserAlreadyAdded, cancellationToken);

            return new ResponseBase<UpdateAppOperationClaimCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde güncellendi." };
        }
    }
}