using TradeCore.AuthService.Models.Request;
using TradeCore.AuthService.Models.Response;
using MediatR;

namespace TradeCore.AuthService.AplicationService
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, ResponseBase<TResponse>>
         where TRequest : RequestBase<TResponse>
    {
        public abstract Task<ResponseBase<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
