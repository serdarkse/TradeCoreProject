using MediatR;
using TradeCore.NotificationService.Models.Request;
using TradeCore.NotificationService.Models.Response;

namespace TradeCore.NotificationService.AplicationService
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, ResponseBase<TResponse>>
        where TRequest : RequestBase<TResponse>
    {
        public abstract Task<ResponseBase<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
