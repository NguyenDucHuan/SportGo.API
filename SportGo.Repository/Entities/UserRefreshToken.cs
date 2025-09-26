using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class UserRefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key

        [Required]
        [StringLength(256)]
        public string Token { get; set; }

        [Required]
        [StringLength(100)]
        public string JwtId { get; set; } // ID của Access Token

        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        [StringLength(255)]
        public string DeviceId { get; set; }

        [StringLength(100)]
        public string DeviceName { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
