using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class VerifyEmailDto
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mã OTP là bắt buộc.")]
        [StringLength(6)]
        public string Otp { get; set; }
    }
}
