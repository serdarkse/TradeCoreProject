using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Models.Dtos;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public interface IAppCustomerService
    {
        Task<AppCustomerDto> CreateUser(CreateAppCustomerCommandRequest request, CancellationToken cancellationToken);
        Task<AppCustomerDto> UpdateUser(UpdateAppCustomerCommandRequest request, AppCustomer user, CancellationToken cancellationToken);
        Task<AppCustomerDto> DeleteUser(DeleteAppCustomerCommandRequest request, CancellationToken cancellationToken);
    }
}
