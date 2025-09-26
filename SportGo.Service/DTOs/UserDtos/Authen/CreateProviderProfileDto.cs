using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.Authen
{
    public class CreateProviderProfileDto
    {
        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string Address { get; set; }

        public string? BusinessPhoneNumber { get; set; }


        public string? BusinessLicenseUrl { get; set; }

        [Required]
        public string BankAccountName { get; set; }

        [Required]
        public string BankAccountNumber { get; set; }

        [Required]
        public string BankName { get; set; }
    }
}
