namespace TradeCore.OrderService.Extensions
{
    public static class DecimalExtension
    {
        public static decimal ToTwoDecimalPlaces(this decimal value)
        {
            return Math.Truncate(100 * value) / 100;
        }
    }
}
