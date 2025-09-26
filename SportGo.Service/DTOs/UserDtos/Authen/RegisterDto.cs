using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }
        [Required]
        public bool IsProvider { get; set; } = false;

    }
}
