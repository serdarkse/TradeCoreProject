using Autofac;
using TradeCore.AuthService.Models.Response;
using MediatR;
using System.Reflection;

namespace TradeCore.AuthService.Container.Decorator
{
    public abstract class DecoratorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
             where TResponse : ResponseBase
             where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

        protected MethodBase GetHandlerMethodInfo()
        {
            var handler = Bootstrapper.Container.Resolve<IRequestHandler<TRequest, TResponse>>();
            return handler?.GetType().GetMethod("Handle");
        }
    }
}
