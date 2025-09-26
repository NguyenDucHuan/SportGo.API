using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
    }
}
