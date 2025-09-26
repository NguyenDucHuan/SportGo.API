using SportGo.Repository.Entities;
using SportGo.Service.DTOs.UserDtos.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto registerDto);
        Task<bool> VerifyEmailAsync(VerifyEmailDto verifyDto);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
        Task<User> GetCurrentUserAsync(int userId);
        Task RequestForgotPasswordAsync(ForgotPasswordDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task SubmitProviderProfileAsync(int userId, CreateProviderProfileDto profileDto);
        Task<bool> ResendVerificationEmailAsync(string email);
    }
}
