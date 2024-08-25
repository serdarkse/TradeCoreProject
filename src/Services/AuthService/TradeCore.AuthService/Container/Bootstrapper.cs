using Autofac;

namespace TradeCore.AuthService.Container
{
    public class Bootstrapper
    {
        public static ILifetimeScope Container { get; set; }

        public static void SetContainer(ILifetimeScope lifetimeScope)
        {
            Container = lifetimeScope;
        }
    }
}
