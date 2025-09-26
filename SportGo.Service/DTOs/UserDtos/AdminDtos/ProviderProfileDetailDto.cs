using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.AdminDtos
{
    public class ProviderProfileDetailDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProviderStatus { get; set; }

        // Thông tin từ ProviderProfile
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string? BusinessLicenseUrl { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
    }
}
