using SportGo.Repository.Paginate;
using SportGo.Service.DTOs.UserDtos.AdminDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.Services.Interfaces
{
    public interface IAdminService
    {
        Task ApproveProviderAsync(int userId);
        Task RejectProviderAsync(int userId, RejectProfileDto rejectDto);
        Task<IPaginate<ProviderPendingDto>> GetFilteredProvidersAsync(ProviderFilterDto filter);
        Task<ProviderProfileDetailDto> GetProviderProfileDetailAsync(int userId);
    }
}
