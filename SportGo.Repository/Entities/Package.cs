using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int DurationInMonths { get; set; }

        public int NormalTurns { get; set; }
        public int PriorityTurns { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation property
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
    }
}
