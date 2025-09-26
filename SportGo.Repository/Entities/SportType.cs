using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class SportType
    {
        [Key]
        public int SportTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        // Navigation property
        public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
    }
}
