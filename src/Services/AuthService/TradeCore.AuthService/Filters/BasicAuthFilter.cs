using TradeCore.AuthService.CrossCuttingConcerns.Caching;
using TradeCore.AuthService.Dependency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Principal;

namespace TradeCore.AuthService.Filters
{
    public class BasicAuthFilter : System.Attribute, IAuthorizationFilter
    {

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (context.HttpContext.Request.Path.Value.Contains("login") || context.HttpContext.Request.Path.Value.Contains("LoginLogouts"))
                {
                    return;
                }

                string authHeader = context.HttpContext.Request.Headers["SessionId"];
                if (authHeader != null)
                {
                    authHeader = authHeader.Substring(1);
                    authHeader = authHeader.Substring(0, authHeader.Length - 1);
                }

                if (authHeader == null)
                {
                    authHeader = context.HttpContext.Request.Headers["Authorization"];

                    if (authHeader!=null)
                    {
                        var handler = new JwtSecurityTokenHandler();
                        authHeader = authHeader.Replace("Bearer ", "");
                        var jsonToken = handler.ReadToken(authHeader);
                        var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                        var sessionId = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

                        authHeader = sessionId;
                    }
                }

                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);

                    var sessionId = authHeaderValue.ToString();

                    var cacheManager = DependencyModule.Resolve<ICacheManager>();

                    var user = await cacheManager.Get($"{CacheKeys.UserSession}={sessionId}");

                    if (user != null)
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(sessionId), null);
                        return;
                    }
                    else
                    {
                        IHttpContextAccessor accessor = DependencyModule.Resolve<IHttpContextAccessor>();

                        var userId = accessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
                        var userSession = await cacheManager.Get<string>($"{CacheKeys.UserSessionForUserId}={userId}");

                        if (userSession == null)
                        {
                            context.Result = new UnauthorizedResult();
                        }
                        else
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userSession), null);

                            return;
                        }
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
