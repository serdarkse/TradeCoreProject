using Autofac;
using TradeCore.NotificationService.Models.Response;
using MediatR;
using System.Reflection;

namespace TradeCore.NotificationService.Container.Decorator
{
    public abstract class DecoratorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TResponse : ResponseBase
            where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

        //TODO: T.K kullanılacak sonra 
        protected MethodBase GetHandlerMethodInfo()
        {
            var handler = Bootstrapper.Container.Resolve<IRequestHandler<TRequest, TResponse>>();
            return handler?.GetType().GetMethod("Handle");
        }
    }
}