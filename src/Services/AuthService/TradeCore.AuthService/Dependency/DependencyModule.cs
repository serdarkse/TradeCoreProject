﻿using TradeCore.AuthService.CrossCuttingConcerns.Caching;
using TradeCore.AuthService.CrossCuttingConcerns.Caching.Redis;

namespace TradeCore.AuthService.Dependency
{
    public class DependencyModule
    {
        private static IServiceCollection services { get; set; }
        public static IConfiguration configuration { get; set; }

        public static void RegisterServices(IServiceCollection _services, IConfiguration _configuration)
        {
            services = _services;
            configuration = _configuration;

            services.AddSingleton<ICacheManager>(new RedisCacheManager(configuration));
        }
        public static T Resolve<T>()
        {
            return services.BuildServiceProvider().GetService<T>();
        }
    }
}
