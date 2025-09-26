using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportGo.Repository;
using SportGo.Repository.Entities;
using SportGo.Repository.Enum;
using SportGo.Repository.Paginate;
using SportGo.Repository.UnitOfWork;
using SportGo.Service.DTOs.UserDtos.AdminDtos;
using SportGo.Service.Services.Interfaces;
using System.Linq.Expressions;

namespace SportGo.Service.Services
{
    public class AdminService : BaseService<AdminService>, IAdminService
    {
        private readonly IMailSenderService _mailSenderService;
        public AdminService(IUnitOfWork<SportGoDbContext> unitOfWork, ILogger<AdminService> logger, IMapper mapper, IMailSenderService mailSenderService) : base(unitOfWork, logger, mapper)
        {
            _mailSenderService = mailSenderService;
        }


        public async Task<IPaginate<ProviderPendingDto>> GetFilteredProvidersAsync(ProviderFilterDto filter)
        {
            Expression<Func<User, bool>> predicate = u =>
            u.Role == nameof(RoleEnum.Provider) &&
            (!filter.Status.HasValue || (u.ProviderStatus.HasValue && u.ProviderStatus == filter.Status)) &&
            (string.IsNullOrWhiteSpace(filter.SearchName) ||
             u.FullName.Contains(filter.SearchName) ||
             u.Email.Contains(filter.SearchName));


            var pagedUsers = await _unitOfWork.GetRepository<User>().GetPagingListAsync(
                selector: user => new ProviderPendingDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    BusinessName = user.ProviderProfile.BusinessName,
                    SubmittedAt = user.ProviderProfile.CreatedAt
                },
                predicate: predicate,
                include: source => source.Include(u => u.ProviderProfile),
                orderBy: source => source.OrderByDescending(u => u.CreatedAt),
                page: filter.PageNumber,
                size: filter.PageSize
            );

            return pagedUsers;
        }

        public async Task RejectProviderAsync(int userId, RejectProfileDto rejectDto)
        {
            var user = await GetProviderUserAsync(userId);
            if (user == null || user.Role != nameof(RoleEnum.Provider))
            {
                throw new KeyNotFoundException("Không tìm thấy chủ sân với ID này.");
            }

            if (user.ProviderStatus != ProviderStatus.Pending)
            {
                throw new InvalidOperationException("Chỉ có thể phê duyệt hồ sơ đang ở trạng thái 'Chờ duyệt'.");
            }

            user.ProviderStatus = ProviderStatus.Rejected;
            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            var replacements = new Dictionary<string, string>
            {
                { "UserName", user.FullName },
                { "BusinessName", user.ProviderProfile?.BusinessName ?? "Cơ sở của bạn" },
                { "RejectReason", rejectDto.Reason }
            };

            await _mailSenderService.SendEmailWithTemplateAsync(
                user.Email,
                "Thông báo về hồ sơ chủ sân của bạn",
                "ProviderRejected",
                replacements
            );
        }
        public async Task ApproveProviderAsync(int userId)
        {
            var user = await GetProviderUserAsync(userId);
            if (user == null || user.Role != nameof(RoleEnum.Provider))
            {
                throw new KeyNotFoundException("Không tìm thấy chủ sân với ID này.");
            }

            if (user.ProviderStatus != (Repository.Enum.ProviderStatus?)ProviderStatus.Pending)
            {
                throw new InvalidOperationException("Chỉ có thể phê duyệt hồ sơ đang ở trạng thái 'Chờ duyệt'.");
            }
            user.ProviderStatus = (Repository.Enum.ProviderStatus?)ProviderStatus.Approved;
            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            var replacements = new Dictionary<string, string>
            {
                { "UserName", user.FullName },
                { "BusinessName", user.ProviderProfile?.BusinessName ?? "Cơ sở của bạn" },
                { "LoginUrl", "" }
            };

            await _mailSenderService.SendEmailWithTemplateAsync(
                user.Email,
                "Chúc mừng! Hồ sơ của bạn đã được phê duyệt!",
                "ProviderApproved",
                replacements
             );
        }


        private async Task<User> GetProviderUserAsync(int userId)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.UserId == userId && u.Role == "Provider",
                include: source => source.Include(u => u.ProviderProfile));
            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy chủ sân với ID này.");
            }
            return user;
        }

        public async Task<ProviderProfileDetailDto> GetProviderProfileDetailAsync(int userId)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
                predicate: u => u.UserId == userId && u.Role == "Provider",
                include: source => source.Include(u => u.ProviderProfile)
                                         .Include(p => p.Facilities));

            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy chủ sân với ID này.");
            }

            return _mapper.Map<ProviderProfileDetailDto>(user);
        }
    }
}
