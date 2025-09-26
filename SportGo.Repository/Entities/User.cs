using SportGo.Repository.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // "User", "Provider", "Admin"

        public string? OtpCode { get; set; } // Dấu ? cho phép giá trị null
        public DateTime? OtpExpiresAt { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        [MaxLength(20)]
        public ProviderStatus? ProviderStatus { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Facility> Facilities { get; set; } = new List<Facility>();
        public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new List<UserRefreshToken>();
        public virtual ProviderProfile? ProviderProfile { get; set; }
    }
}
