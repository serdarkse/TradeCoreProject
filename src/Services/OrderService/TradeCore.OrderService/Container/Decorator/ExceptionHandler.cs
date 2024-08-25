using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Models.Response;
using MediatR;
using Newtonsoft.Json;

namespace TradeCore.OrderService.Container.Decorator
{
    public class ExceptionHandler<TRequest, TResponse> : DecoratorBase<TRequest, TResponse>
             where TRequest : IRequest<TResponse>
             where TResponse : ResponseBase, new()
    {
        private readonly ILogger<ExceptionHandler<TRequest, TResponse>> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)

        {
            TResponse response;
            try
            {
                response = await next();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Unhandled exception occured while processing request {JsonConvert.SerializeObject(request)}");
                switch (exception)
                {
                    case BusinessRuleException businessRuleException:
                        response = new TResponse
                        {
                            UserMessage = businessRuleException.UserMessage,
                            Message = businessRuleException.Message,
                            MessageCode = businessRuleException.Code
                        };
                        break;
                    //http timeout
                    case TaskCanceledException:
                    case AggregateException:
                        response = new TResponse
                        {
                            MessageCode = ApplicationMessage.TimeoutOccurred.Code(),
                            Message = ApplicationMessage.TimeoutOccurred.Message(),
                            UserMessage = ApplicationMessage.TimeoutOccurred.UserMessage()
                        };
                        break;
                    //unexpected http response received
                    case HttpRequestException:
                    case JsonReaderException:
                        response = new TResponse
                        {
                            MessageCode = ApplicationMessage.UnExpectedHttpResponseReceived.Code(),
                            Message = ApplicationMessage.UnExpectedHttpResponseReceived.Message(),
                            UserMessage = ApplicationMessage.UnExpectedHttpResponseReceived.UserMessage()
                        };
                        break;
                    default:
                        response = new TResponse
                        {
                            MessageCode = ApplicationMessage.UnhandledError.Code(),
                            Message = ApplicationMessage.UnhandledError.Message(),
                            UserMessage = ApplicationMessage.UnhandledError.UserMessage()
                        };
                        break;
                }
            }
            return response;
        }
    }
}
