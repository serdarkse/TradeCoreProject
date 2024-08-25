using Autofac;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Helpers;
using TradeCore.AuthService.Models.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using ILogger = Serilog.ILogger;

namespace TradeCore.AuthService.Attribute
{
    [ExcludeFromCodeCoverage]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;
        public ValidateModelAttribute(ILogger logger)
        {
            _logger = logger;
        }
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var exceptionFeature = context.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionFeature != null)
                {
                    _logger.Error("Hata meydana geldi. hata: " + exceptionFeature.Error.Message, exceptionFeature.Error, GetErrorMessageFromContext(context));
                }

                var response = new ResponseBase<object>()
                {
                    Success = false,
                    MessageCode = ApplicationMessage.InvalidParameter.Code(),
                    Message = ApplicationMessage.InvalidParameter.Message(),
                    UserMessage = ApplicationMessage.InvalidParameter.UserMessage()
                };
                context.Result = new OkObjectResult(response);
            }
            else
            {
                var user = await UserHelper.User;
                var logDetail = new
                {
                    UserName = user.Name,
                    IPAddress = Convert.ToString(ipHostInfo.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork)),
                    ControllerAccess = (string)context.RouteData.Values["controller"],
                    ActionAccess = (string)context.RouteData.Values["action"],
                    Timestamp = DateTimeHelper.DateTimeUtcTimeZone(),

                };
                _logger.Information(JsonConvert.SerializeObject(logDetail));
            }
        }

        private static string GetErrorMessageFromContext(ActionExecutingContext context)
        {
            var errorMessage = string.Empty;

            var errorCollectionList = context.ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            foreach (var errorCollection in errorCollectionList)
            {
                foreach (var error in errorCollection)
                {
                    if (error.Exception != null)
                        errorMessage += error.Exception.Message;
                    else
                        errorMessage += error.ErrorMessage;
                }
            }

            return errorMessage;
        }
    }
}
