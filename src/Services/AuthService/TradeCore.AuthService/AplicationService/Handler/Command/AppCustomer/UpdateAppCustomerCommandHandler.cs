using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command.AppCustomer
{
    public class UpdateAppCustomerCommandHandler : RequestHandlerBase<UpdateAppCustomerCommandRequest, UpdateAppCustomerCommandResponse>
    {
        private readonly IAppCustomerService _appCustomerService;
        private readonly IAppCustomerRepository _appCustomerRepository;
        public UpdateAppCustomerCommandHandler(IAppCustomerRepository appCustomerRepository, IAppCustomerService appCustomerService)
        {
            _appCustomerService = appCustomerService;
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<UpdateAppCustomerCommandResponse>> Handle(UpdateAppCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appCustomerRepository.FindByAsync(p => p.Address == request.Email, cancellationToken);

            if (isUserAlreadyAdded == null)
                throw new BusinessRuleException(ApplicationMessage.InvalidId.Code(), ApplicationMessage.InvalidId.Message(), ApplicationMessage.InvalidId.UserMessage());

            var createdUser = await _appCustomerService.UpdateUser(request, isUserAlreadyAdded, cancellationToken);

            return new ResponseBase<UpdateAppCustomerCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde güncellendi." };
        }
    }
}