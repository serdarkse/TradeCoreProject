using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command
{
    public class DeleteAppCustomerClaimCommandHandler : RequestHandlerBase<DeleteAppCustomerClaimCommandRequest, DeleteAppCustomerClaimCommandResponse>
    {
        private readonly IAppCustomerClaimService _appCustomerClaimService;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        public DeleteAppCustomerClaimCommandHandler(IAppCustomerClaimRepository appCustomerClaimRepository, IAppCustomerClaimService appCustomerClaimService)
        {
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appCustomerClaimService = appCustomerClaimService;
        }

        public override async Task<ResponseBase<DeleteAppCustomerClaimCommandResponse>> Handle(DeleteAppCustomerClaimCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appCustomerClaimRepository.FindByAsync(p => p.Id == request.Id, cancellationToken);

            if (isUserAlreadyAdded == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            var createdUser = await _appCustomerClaimService.DeleteappCustomerClaim(request, cancellationToken);

            return new ResponseBase<DeleteAppCustomerClaimCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde silindi." };
        }
    }
}