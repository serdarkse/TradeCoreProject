namespace TradeCore.AuthService.Helpers.HelperModels
{
    class TimeoutOptions
    {
        public int TimeoutSeconds { get; set; } = 60; // seconds
        public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);
    }
}
