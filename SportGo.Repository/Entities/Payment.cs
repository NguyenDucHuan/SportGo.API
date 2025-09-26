using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; } // "PackagePurchase", "BookingDeposit"

        public int ReferenceId { get; set; } // Could be UserPackageId or BookingId

        [StringLength(50)]
        public string PaymentMethod { get; set; } // "VNPAY", "MoMo"

        [StringLength(100)]
        public string TransactionCode { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // "Succeeded", "Failed"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
