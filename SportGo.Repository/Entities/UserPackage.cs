using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class UserPackage
    {
        [Key]
        public int UserPackageId { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key

        [Required]
        public int PackageId { get; set; } // Foreign Key

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingNormalTurns { get; set; }
        public int RemainingPriorityTurns { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
