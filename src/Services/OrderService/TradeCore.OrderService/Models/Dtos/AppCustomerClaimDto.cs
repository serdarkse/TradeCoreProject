namespace TradeCore.OrderService.Models.Dtos
{
    public class AppCustomerClaimDto
    {
        public Guid AppCustomerId { get; set; }
        public Guid AppOperationClaimId { get; set; }

        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
    }
}
