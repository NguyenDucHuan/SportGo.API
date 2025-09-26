using AutoMapper;
using SportGo.Repository.Entities;
using SportGo.Service.DTOs.UserDtos.AdminDtos;
using SportGo.Service.DTOs.UserDtos.Authen;
namespace SportGo.API.MapperProfile
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserProfileDto>()
               .ForMember(
                   dest => dest.CurrentPackage,
                   opt => opt.MapFrom(src => src.UserPackages
                                               .FirstOrDefault(p => p.IsActive && p.EndDate >= DateTime.UtcNow))
               );
            CreateMap<UserPackage, UserPackageInfoDto>()
                .ForMember(
                    dest => dest.PackageName,
                    opt => opt.MapFrom(src => src.Package.Name)
                );
            CreateMap<CreateProviderProfileDto, ProviderProfile>();

            CreateMap<User, ProviderPendingDto>()
                .ForMember(dest => dest.BusinessName, opt => opt.MapFrom(src => src.ProviderProfile.BusinessName))
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => src.ProviderProfile.CreatedAt));

            // Ánh xạ cho chi tiết hồ sơ
            CreateMap<User, ProviderProfileDetailDto>()
                .ForMember(dest => dest.ProviderStatus, opt => opt.MapFrom(src => src.ProviderStatus.ToString()))
                .IncludeMembers(src => src.ProviderProfile); // Kết hợp thông tin từ ProviderProfile

            CreateMap<ProviderProfile, ProviderProfileDetailDto>();
        }
    }
}
