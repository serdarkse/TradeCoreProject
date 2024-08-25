using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command.AppCustomer
{
    public class DeleteappCustomerCommandHandler : RequestHandlerBase<DeleteAppCustomerCommandRequest, DeleteAppCustomerCommandResponse>
    {
        private readonly IAppCustomerService _appCustomerService;
        private readonly IAppCustomerRepository _appCustomerRepository;
        public DeleteappCustomerCommandHandler(IAppCustomerRepository appCustomerRepository, IAppCustomerService appCustomerService)
        {
            _appCustomerService = appCustomerService;
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<DeleteAppCustomerCommandResponse>> Handle(DeleteAppCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appCustomerRepository.FindByAsync(p => p.Id == request.CustomerId, cancellationToken);

            if (isUserAlreadyAdded == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            var createdUser = await _appCustomerService.DeleteUser(request, cancellationToken);

            return new ResponseBase<DeleteAppCustomerCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde silindi." };
        }
    }
}