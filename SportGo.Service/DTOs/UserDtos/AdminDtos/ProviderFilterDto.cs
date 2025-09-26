using SportGo.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.DTOs.UserDtos.AdminDtos
{
    public class ProviderFilterDto
    {
        // Tham số phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchName { get; set; }
        public ProviderStatus? Status { get; set; }
    }

}
