using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Domain.BaseEntity;

namespace TradeCore.AuthService.Domain.AppCustomerAggregate
{
    public partial class AppCustomer : Entity
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250, ErrorMessage = "Address can't be longer than 250 characters.")]
        public string Address { get; set; }
        public string SessionId { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsSystemAdmin { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public string AuthenticationProviderType { get; set; } = "Person";

        public virtual ICollection<AppCustomerClaim> AppCustomerClaims { get; set; }

        public AppCustomer()
        {
            Status = true;
        }
    }
}
