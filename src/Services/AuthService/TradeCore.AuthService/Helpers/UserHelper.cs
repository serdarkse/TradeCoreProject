using TradeCore.AuthService.CrossCuttingConcerns.Caching;
using TradeCore.AuthService.Dependency;
using TradeCore.AuthService.Models.Dtos;

namespace TradeCore.AuthService.Helpers
{
    public class UserHelper
    {
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

                var sessionId = accessor.HttpContext.User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.SerialNumber)?.Value;
                var cacheManager = DependencyModule.Resolve<ICacheManager>();

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
                        return new AppCustomerDto();
                    }
                    else
                    {
                        return user;
                    }
                }
            }
            catch (Exception ex)
            {
                return new AppCustomerDto();
            }
        }
    }
}
