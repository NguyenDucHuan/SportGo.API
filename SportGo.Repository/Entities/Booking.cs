using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int UserPackageId { get; set; } // Foreign Key

        [Required]
        public int CourtId { get; set; } // Foreign Key

        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [Required]
        [StringLength(20)]
        public string BookingType { get; set; } // "Normal", "Priority"

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // "Pending", "Confirmed", "Cancelled", "Completed"

        public bool DepositRequired { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DepositAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserPackageId")]
        public virtual UserPackage UserPackage { get; set; }

        [ForeignKey("CourtId")]
        public virtual Court Court { get; set; }
    }
}
