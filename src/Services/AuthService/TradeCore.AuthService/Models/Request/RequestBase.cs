using TradeCore.AuthService.Models.Response;
using MediatR;

namespace TradeCore.AuthService.Models.Request
{
    public abstract class RequestBase<TResponse> : IRequest<ResponseBase<TResponse>>
    {
    }
}
