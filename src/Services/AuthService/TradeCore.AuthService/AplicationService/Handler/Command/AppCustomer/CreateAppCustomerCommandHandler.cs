using TradeCore.AuthService.AplicationService.Handler.Service;
using TradeCore.AuthService.Container.Decorator;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command.AppCustomer;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command.AppCustomer
{
    public class CreateAppCustomerCommandHandler : RequestHandlerBase<CreateAppCustomerCommandRequest, CreateAppCustomerCommandResponse>
    {
        private readonly IAppCustomerService _appCustomerService;
        private readonly IAppCustomerRepository _appCustomerRepository;
        public CreateAppCustomerCommandHandler(IAppCustomerRepository appCustomerRepository, IAppCustomerService appCustomerService)
        {
            _appCustomerService = appCustomerService;
            _appCustomerRepository = appCustomerRepository;
        }

        public override async Task<ResponseBase<CreateAppCustomerCommandResponse>> Handle(CreateAppCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var isUserAlreadyAdded = await _appCustomerRepository.FindByAsync(p => p.Email == request.Email, cancellationToken);

            if (isUserAlreadyAdded != null)
                throw new BusinessRuleException(ApplicationMessage.AllreadyAdded.Code(), ApplicationMessage.AllreadyAdded.Message(), ApplicationMessage.AllreadyAdded.UserMessage());

            var createdUser = await _appCustomerService.CreateUser(request, cancellationToken);

            return new ResponseBase<CreateAppCustomerCommandResponse> { Success = true, UserMessage = "Başarılı bir şekilde kaydedildi." };
        }
    }
}