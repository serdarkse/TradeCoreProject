using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command
{
    public class CreateAppOperationClaimCommandHandler : RequestHandlerBase<CreateAppOperationClaimCommandRequest, CreateAppOperationClaimCommandResponse>
    {
        private readonly IAppOperationClaimService _appOperationClaimService;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public CreateAppOperationClaimCommandHandler(IAppOperationClaimRepository appOperationClaimRepository, IAppOperationClaimService appOperationClaimService)
        {
            _appOperationClaimService = appOperationClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<CreateAppOperationClaimCommandResponse>> Handle(CreateAppOperationClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appOperationClaimRepository.FindByAsync(p => p.FunctionName == request.FunctionName, cancellationToken);

            if (isUserAlreadyAdded != null)
                throw new BusinessRuleException(ApplicationMessage.AllreadyAdded.Code(), ApplicationMessage.AllreadyAdded.Message(), ApplicationMessage.AllreadyAdded.UserMessage());

            var createdUser = await _appOperationClaimService.CreateAppOperationClaim(request, cancellationToken);

            return new ResponseBase<CreateAppOperationClaimCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde kaydedildi." };
        }
    }
}