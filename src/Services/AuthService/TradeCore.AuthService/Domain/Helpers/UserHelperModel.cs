namespace TradeCore.AuthService.Domain.Helpers
{
    public class UserHelperModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsAdUser { get; set; } = false;

    }
}
