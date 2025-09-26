using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.AdminDtos
{
    public class RejectProfileDto
    {
        [Required(ErrorMessage = "Lý do từ chối là bắt buộc.")]
        [MaxLength(500)]
        public string Reason { get; set; }
    }
}
