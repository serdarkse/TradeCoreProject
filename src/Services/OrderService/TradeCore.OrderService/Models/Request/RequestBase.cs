using TradeCore.OrderService.Models.Response;
using MediatR;

namespace TradeCore.OrderService.Models.Request
{
    public abstract class RequestBase<TResponse> : IRequest<ResponseBase<TResponse>>
    {
    }
}
