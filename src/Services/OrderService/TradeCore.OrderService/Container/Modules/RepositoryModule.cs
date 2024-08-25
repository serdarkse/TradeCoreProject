using Autofac;
using TradeCore.OrderService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Module = Autofac.Module;

namespace TradeCore.OrderService.Container.Modules
{
    public class RepositoryModule : Module
    {
        private static string _connectionString;

        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _connectionString = configuration["DbConnString"];
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<OrderDbContext>(options => options.UseSqlServer(_connectionString), ServiceLifetime.Scoped);


        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor)).AsSelf().InstancePerLifetimeScope();

            var assemblyType = typeof(GenericRepository<>).GetTypeInfo();
            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => x != typeof(OrderDbContext))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
