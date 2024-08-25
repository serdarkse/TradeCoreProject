using TradeCore.NotificationService.Models.Response;
using MediatR;

namespace TradeCore.NotificationService.Models.Request
{
    public abstract class RequestBase<TResponse> : IRequest<ResponseBase<TResponse>>
    {
    }
}