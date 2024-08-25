namespace TradeCore.OrderService.Utilities.Mail
{
    public interface IMailService
    {
        void Send(EmailMessage emailMessage);
    }
}
