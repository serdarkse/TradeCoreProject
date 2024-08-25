using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TradeCore.AuthService.Helpers;
using TradeCore.AuthService.Models.Request.Command.AppLdapUser;
using TradeCore.AuthService.Models.Request.Query.Auth;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Command.AppLdapUser;
using TradeCore.AuthService.Models.Response.Query.Auth;

namespace TradeCore.AuthService.Controllers
{
    [ApiController]
    [Produces("application/json", "text/plain")]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<AuthLoginServiceResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginServiceRequest request, CancellationToken cancellationToken)
        {

            var response = await _mediator.Send(request, cancellationToken);

            if (!response.Success)
            {
                return Ok(response);
            }

            var usermodel = response.Data.CustomerInfo;

            ClaimsIdentity identity = null;
            identity = new ClaimsIdentity(new[]
                                 {
                        new Claim(ClaimTypes.Name, usermodel.Name),
                        new Claim(ClaimTypes.SerialNumber,usermodel.SessionId),
                        new Claim(ClaimTypes.Role, usermodel.IsSystemAdmin ? nameof(ProfileForCookie.Admin) : nameof(ProfileForCookie.User))
                    },
                                 CookieAuthenticationDefaults.AuthenticationScheme
                                 );

            _ = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(identity),
                                                new AuthenticationProperties
                                                {
                                                    AllowRefresh = true,
                                                    IssuedUtc = DateTimeHelper.DateTimeUtcTimeZone(),
                                                    ExpiresUtc = DateTimeHelper.DateTimeUtcTimeZone().AddDays(365),
                                                    IsPersistent = true
                                                });

            _logger.LogInformation($"Login İşlemi başarılı. Login olan kullanıcı bilgileri : {JsonConvert.SerializeObject(usermodel)}");

            return Ok(response);
        }

        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<LogOffUserQueryResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost("logoff")]
        public async Task<IActionResult> LogOff([FromBody] LogOffUserQueryRequest username, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(username, cancellationToken);

            _ = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);

        }

        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseBase<RegisterUserCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommandRequest createUser, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createUser, cancellationToken);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
