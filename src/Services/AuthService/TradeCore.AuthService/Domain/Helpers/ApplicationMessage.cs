namespace TradeCore.AuthService.Domain.Helpers
{
    public static class ApplicationMessage
    {
        private const string CommonUserErrorMessage =
            "İşleminiz şu anda gerçekleştirilemiyor. Lütfen daha sonra tekrar deneyiniz.";
        private const string ApiCode = "";
        public static string UnhandledError = "CAM_-1";
        public static readonly string Success = "CAM_0";
        public static string InvalidParameter = "CAM_1";
        public static string TimeoutOccurred = "CAM_2";
        public static string UnExpectedHttpResponseReceived = "CAM_3";
        public static string AllreadyAdded = "CAM_4";
        public static string IsClaimExists = "CAM_5";
        public static string IsGroupExists = "CAM_6";
        public static string IsGroupClaimExists = "CAM_7";
        public static string ClaimIdIsNotFoundInClaimTable = "CAM_8";
        public static string ThisClaimIsDefinedThisUser = "CAM_9";
        public static string ClaimIdIsAlreadyDefinedForThisUser = "CAM_10";
        public static string InvalidId = "CAM_20";

        private static readonly Dictionary<string, string> ErrorMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, "Unhandled exception."},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {InvalidParameter, "Invalid parameter."},
                {TimeoutOccurred, "Timeout oluştu."},
                {UnExpectedHttpResponseReceived, "Beklenmedik bir httpCode ile response alındı."},
                {AllreadyAdded, "İlgili veri daha önce zaten eklenmiş."},
                {IsClaimExists, "Yetki bulunamadı."},
                {IsGroupExists, "Grup bulunamadı."},
                {IsGroupClaimExists, "Grup Yetkisi bulunamadı."},
                {ClaimIdIsNotFoundInClaimTable, "Yetki tablosunda ilgili yetki bulunamadı."},
                {ThisClaimIsDefinedThisUser, "İlgili yetki daha önce zaten eklenmiş."},
                {ClaimIdIsAlreadyDefinedForThisUser, "İlgili yetki bu kullanıcı için zaten daha önceden eklenmiş."},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},

            };

        private static readonly Dictionary<string, string> UserMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, CommonUserErrorMessage},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {InvalidParameter, CommonUserErrorMessage},
                {TimeoutOccurred, CommonUserErrorMessage},
                {UnExpectedHttpResponseReceived, CommonUserErrorMessage},
                {AllreadyAdded, CommonUserErrorMessage},
                {IsClaimExists, CommonUserErrorMessage},
                {IsGroupExists, CommonUserErrorMessage},
                {IsGroupClaimExists, CommonUserErrorMessage},
                {ClaimIdIsNotFoundInClaimTable, CommonUserErrorMessage},
                {ThisClaimIsDefinedThisUser, CommonUserErrorMessage},
                {ClaimIdIsAlreadyDefinedForThisUser, CommonUserErrorMessage},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},
            };
        public static string Code(this string code)
        {
            return $"{code}";
        }
        public static string Message(this string code, params object[] messageParams)
        {
            ErrorMessages.TryGetValue(code, out var errorMessage);
            return string.Format(errorMessage, messageParams);
        }
        public static string UserMessage(this string code, params object[] messageParams)
        {
            UserMessages.TryGetValue(code, out var errorMessage);
            return string.Format(errorMessage, messageParams);
        }
    }
}
