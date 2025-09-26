using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.AdminDtos
{
    public class ProviderPendingDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
