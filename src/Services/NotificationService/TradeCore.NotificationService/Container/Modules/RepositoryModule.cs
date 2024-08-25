using Autofac;
using Module = Autofac.Module;

namespace TradeCore.NotificationService.Container.Modules
{
    public class RepositoryModule : Module
    {
        private static string _connectionString;

        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _connectionString = configuration["DbConnString"];
            //serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<NotificationDbContext>(options => options.UseSqlServer(_connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(HttpContextAccessor)).As(typeof(IHttpContextAccessor)).AsSelf().InstancePerLifetimeScope();

            //var assemblyType = typeof(GenericRepository<>).GetTypeInfo();
            //builder.RegisterAssemblyTypes(assemblyType.Assembly)
            //    .Where(x => x != typeof(NotificationDbContext))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}