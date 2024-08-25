namespace TradeCore.AuthService.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime DateTimeUtcTimeZone()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istanbulTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Istanbul");
            DateTime istanbulTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istanbulTimeZone);

            return istanbulTime;
        }
    }
}
