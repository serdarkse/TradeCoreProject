namespace TradeCore.OrderService.Domain.Helpers
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
        public static string IncorrectApiKeyId = "9999";
        public static string NotExistCustomerId = "CAM_CRM_2";
        public static string InvalidId = "CAM_3";
        public static string ExistProduct = "CAM_4";
        public static string ExistCustomerOrder = "CAM_5";
        public static string NotExistProduct = "CAM_CRM_6";


        private static readonly Dictionary<string, string> ErrorMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, "Unhandled exception."},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {InvalidParameter, "Invalid parameter."},
                {TimeoutOccurred, "Timeout oluştu."},
                {UnExpectedHttpResponseReceived, "Beklenmedik bir httpCode ile response alındı."},
                {NotExistCustomerId, "CustomerId bulunamadı!."},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},
                {IncorrectApiKeyId, "Hatalı Api Id/Key"},
                {ExistProduct, "Ürün zaten kayıtlı"},
                {ExistCustomerOrder, "CustomerOrder bulunamadı"},
                {NotExistProduct, "CustomerOrder bulunamadı"},
            };

        private static readonly Dictionary<string, string> UserMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, CommonUserErrorMessage},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {InvalidParameter, CommonUserErrorMessage},
                {TimeoutOccurred, CommonUserErrorMessage},
                {UnExpectedHttpResponseReceived, CommonUserErrorMessage},
                {NotExistCustomerId, "CustomerId bulunamadı!."},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},
                {IncorrectApiKeyId, "Hatalı Api Id/Key"},
                {ExistProduct, "Ürün Bulunamadı"},
                {ExistCustomerOrder, "CustomerOrder bulunamadı"},
                {NotExistProduct, "CustomerOrder bulunamadı"},


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
