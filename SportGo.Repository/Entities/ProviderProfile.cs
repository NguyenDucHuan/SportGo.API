using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.Entities
{
    public class ProviderProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Khóa ngoại

        [Required]
        [MaxLength(200)]
        public string BusinessName { get; set; } // Tên cơ sở kinh doanh

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string BusinessPhoneNumber { get; set; } // SĐT công khai của sân

        public string? BusinessLicenseUrl { get; set; } // URL ảnh giấy phép kinh doanh

        [MaxLength(100)]
        public string BankAccountName { get; set; } // Tên chủ tài khoản

        [MaxLength(20)]
        public string BankAccountNumber { get; set; } // Số tài khoản

        [MaxLength(100)]
        public string BankName { get; set; } // Tên ngân hàng

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Mối quan hệ 1-1 ---
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
