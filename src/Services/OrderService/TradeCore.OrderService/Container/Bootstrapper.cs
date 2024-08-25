using Autofac;

namespace TradeCore.OrderService.Container
{
    public class Bootstrapper
    {
        //TODO: T.K kullanılacak sonra 
        public static ILifetimeScope Container { get; set; }

        public static void SetContainer(ILifetimeScope lifetimeScope)
        {
            Container = lifetimeScope;
        }
    }
}
