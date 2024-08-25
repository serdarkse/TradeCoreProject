using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeCore.AuthService.Domain.BaseEntity
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "smallint")]
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? CreatedCustomerId { get; set; }
    }

}