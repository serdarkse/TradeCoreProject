namespace TradeCore.OrderService.Models.Dtos
{
    public class AppCustomerDto
    {
        public Guid AppCustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }
        public bool Status { get; set; }
        public bool IsSystemAdmin { get; set; }
        public string InputType { get; set; }
        public string SessionId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public bool IsException { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionMessageCode { get; set; }
        public List<string> Messages { get; set; }
        public List<AppCustomerClaimDto> AppCustomerClaims { get; set; }

    }
}
