using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }

        [Required]
        public int ProviderId { get; set; } // Foreign Key

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string Description { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("ProviderId")]
        public virtual User Provider { get; set; }
        public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
    }
}
