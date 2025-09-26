using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class Court
    {
        [Key]
        public int CourtId { get; set; }

        [Required]
        public int FacilityId { get; set; } // Foreign Key

        [Required]
        public int SportTypeId { get; set; } // Foreign Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string CourtType { get; set; } // "Normal", "Priority"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DefaultPrice { get; set; }

        // Navigation properties
        [ForeignKey("FacilityId")]
        public virtual Facility Facility { get; set; }

        [ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
