using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.CrossCuttingConcerns.Caching;
using TradeCore.OrderService.Dependency;

namespace TradeCore.OrderService.Helpers
{
    public class UserHelper
    {
        private static Guid ServiceGuidUserId = new Guid("65F6C1C3-FDAF-402F-BF64-F25CB9B46D4A");
        public static Task<AppCustomerDto> User
        {
            get
            {
                return GetUserAsync();
            }
        }
        private static async Task<AppCustomerDto> GetUserAsync()
        {
            try
            {
                IHttpContextAccessor accessor = DependencyModule.Resolve<IHttpContextAccessor>();

                var sessionId = Thread.CurrentPrincipal?.Identity.Name;

                var cacheManager = DependencyModule.Resolve<ICacheManager>();
                if (sessionId == null)
                {
                    var userId = accessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                    var userSession = await cacheManager.Get<string>($"{CacheKeys.UserSessionForUserId}={userId}");

                }

                AppCustomerDto userSessionCache = await cacheManager.Get<AppCustomerDto>($"{CacheKeys.UserSession}={sessionId}");
                if (userSessionCache != null)
                {
                    return userSessionCache;
                }
                else
                {
                    var userId = accessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                    var userSession = await cacheManager.Get<string>($"{CacheKeys.UserSessionForUserId}={userId}");
                    var user = await cacheManager.Get<AppCustomerDto>($"{CacheKeys.UserSession}={userSession}");
                    if (user == null)
                    {
                        return new AppCustomerDto { AppCustomerId = ServiceGuidUserId };
                    }
                    else
                    {
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO : T.K bu kısıma bakılacak şimdilik içeride eren in id si verildi.
                return new AppCustomerDto { AppCustomerId = ServiceGuidUserId };

            }
        }
    }
}
