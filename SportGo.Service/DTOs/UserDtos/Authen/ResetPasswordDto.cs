using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã OTP là bắt buộc.")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
