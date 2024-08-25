using Autofac;
using MediatR;
using System.Reflection;
using TradeCore.NotificationService.AplicationService;
using TradeCore.NotificationService.Container.Decorator;
using TradeCore.NotificationService.Models.Request;
using Module = Autofac.Module;

namespace TradeCore.NotificationService.Container.Modules
{
    public class MediatRModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx => {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder
                .RegisterAssemblyTypes(typeof(RequestBase<>).Assembly)
                .AsClosedTypesOf(typeof(IRequest<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(RequestHandlerBase<,>).Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder
                .RegisterGeneric(typeof(ExceptionHandler<,>))
                .As(typeof(IPipelineBehavior<,>));

            base.Load(builder);
        }
    }
}