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
    public class UpdateAppCustomerClaimCommandHandler : RequestHandlerBase<UpdateAppCustomerClaimCommandRequest, UpdateAppCustomerClaimCommandResponse>
    {
        private readonly IAppCustomerClaimService _appCustomerClaimService;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public UpdateAppCustomerClaimCommandHandler(IAppCustomerClaimRepository appCustomerClaimRepository, IAppCustomerClaimService appCustomerClaimService, IAppOperationClaimRepository appOperationClaimRepository)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appCustomerClaimService = appCustomerClaimService;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<UpdateAppCustomerClaimCommandResponse>> Handle(UpdateAppCustomerClaimCommandRequest request, CancellationToken cancellationToken)
        {

            var isThereClaimRecord = _appOperationClaimRepository.FindByAsync(a => a.Id == request.AppOperationClaimId, cancellationToken) is null;
            if (!isThereClaimRecord)
                throw new BusinessRuleException(ApplicationMessage.ClaimIdIsNotFoundInClaimTable.Code(), ApplicationMessage.ClaimIdIsNotFoundInClaimTable.Message(), ApplicationMessage.ClaimIdIsNotFoundInClaimTable.UserMessage());

            var userClaimRecord = await _appCustomerClaimRepository.FindByAsync(u => u.AppOperationClaimId == request.AppOperationClaimId && u.AppCustomerId == request.AppCustomerId, cancellationToken) is null;
            if (userClaimRecord)
                throw new BusinessRuleException(ApplicationMessage.ClaimIdIsAlreadyDefinedForThisUser.Code(), ApplicationMessage.ClaimIdIsAlreadyDefinedForThisUser.Message(), ApplicationMessage.ClaimIdIsAlreadyDefinedForThisUser.UserMessage());

            var isUserAlreadyAdded = await _appCustomerClaimRepository.FindByAsync(p => p.Id == request.Id, cancellationToken);

            if (isUserAlreadyAdded == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            var createdUser = await _appCustomerClaimService.UpdateappCustomerClaim(request, isUserAlreadyAdded, cancellationToken);

            return new ResponseBase<UpdateAppCustomerClaimCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde güncellendi." };
        }
    }
}