namespace TradeCore.AuthService.CrossCuttingConcerns.Caching
{
    public static class CacheKeys
    {
        public static string UserIdForClaim => "UserIdForClaim";
        public static string UserLang => "UserLang";
        public static string UserSession => "UserSession";
        public static string UserSessionForUserId => "UserSessionForUserId";
        public static string UsersGroupUserList => "UsersGroupUserList";
        public static string UserChildGroupUserList => "UserChildGroupUserList";
        public static string Dashboard => "Admin_Dashboard";
        public static string AllSystemAdmin => "AllSystemAdmin";
    }
}
