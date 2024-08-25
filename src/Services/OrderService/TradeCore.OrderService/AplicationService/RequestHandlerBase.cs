using TradeCore.OrderService.Models.Request;
using TradeCore.OrderService.Models.Response;
using MediatR;

namespace TradeCore.OrderService.AplicationService
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, ResponseBase<TResponse>>
        where TRequest : RequestBase<TResponse>
    {
        public abstract Task<ResponseBase<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
