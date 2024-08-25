namespace TradeCore.NotificationService.Container.Decorator
{
    public class BusinessRuleException : Exception
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string UserMessage { get; set; }

        protected BusinessRuleException()
        {

        }

        public BusinessRuleException(string code, string message, string userMessage) : base(message)
        {
            Code = code;
            Message = message;
            UserMessage = userMessage;
        }
    }
}
