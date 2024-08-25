using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command
{
    public class CreateAppCustomerClaimCommandHandler : RequestHandlerBase<CreateAppCustomerClaimCommandRequest, CreateAppCustomerClaimCommandResponse>
    {
        private readonly IAppCustomerClaimService _appCustomerClaimService;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public CreateAppCustomerClaimCommandHandler(IAppCustomerClaimRepository appCustomerClaimRepository, IAppCustomerClaimService appCustomerClaimService, IAppOperationClaimRepository appOperationClaimRepository)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appCustomerClaimService = appCustomerClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<CreateAppCustomerClaimCommandResponse>> Handle(CreateAppCustomerClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var isThereClaimRecord = _appOperationClaimRepository.FindByAsync(a => a.Id == request.AppOperationClaimId, cancellationToken) is null;
            if (!isThereClaimRecord)
                throw new BusinessRuleException(ApplicationMessage.ClaimIdIsNotFoundInClaimTable.Code(), ApplicationMessage.ClaimIdIsNotFoundInClaimTable.Message(), ApplicationMessage.ClaimIdIsNotFoundInClaimTable.UserMessage());


            var isThereappCustomerClaimRecord = _appCustomerClaimRepository.FindByAsync(u => u.AppCustomerId == request.AppCustomerId && u.AppOperationClaimId == request.AppOperationClaimId, cancellationToken) is null;
            if (isThereappCustomerClaimRecord)
                throw new BusinessRuleException(ApplicationMessage.AllreadyAdded.Code(), ApplicationMessage.AllreadyAdded.Message(), ApplicationMessage.AllreadyAdded.UserMessage());

            var createdUser = await _appCustomerClaimService.CreateappCustomerClaim(request, cancellationToken);

            return new ResponseBase<CreateAppCustomerClaimCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde kaydedildi." };
        }
    }
}