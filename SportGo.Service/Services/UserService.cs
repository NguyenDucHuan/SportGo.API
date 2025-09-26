using AutoMapper;
using MailKit;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto.Generators;
using SportGo.Repository;
using SportGo.Repository.Entities;
using SportGo.Repository.Enum;
using SportGo.Repository.UnitOfWork;
using SportGo.Service.DTOs.UserDtos.Authen;
using SportGo.Service.Services.Interfaces;

namespace SportGo.Service.Services
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly IMailSenderService _mailSenderService;
        private readonly ITokenService _tokenService;
        public UserService(IUnitOfWork<SportGoDbContext> unitOfWork, ILogger<UserService> logger, IMapper mapper, IMailSenderService mailSenderService, ITokenService tokenService) : base(unitOfWork, logger, mapper)
        {
            _mailSenderService = mailSenderService;
            _tokenService = tokenService;
        }




        public async Task<User> GetCurrentUserAsync(int userId)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.UserId == userId);
            if (user == null)
            {
                throw new ArgumentException("Người dùng không tồn tại.");
            }
            return user;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new ArgumentException("Email hoặc mật khẩu không đúng.");
            }
            if (!user.IsEmailVerified)
            {
                throw new ArgumentException("Email chưa được xác thực. Vui lòng kiểm tra hộp thư đến để lấy mã OTP.");
            }
            return await _tokenService.GenerateTokensAsync(user, loginDto.DeviceId, loginDto.DeviceName);
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: w => w.Email == registerDto.Email);
            if (existingUser != null && existingUser.IsEmailVerified)
            {
                throw new ArgumentException("Địa chỉ email này đã được sử dụng.");
            }
            var otp = new Random().Next(100000, 999999).ToString();
            var otpExpiresAt = DateTime.UtcNow.AddMinutes(5);

            var userToRegister = _mapper.Map<User>(registerDto);
            userToRegister.IsEmailVerified = false;
            userToRegister.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            if (registerDto.IsProvider)
            {
                userToRegister.Role = "Provider";
                userToRegister.ProviderStatus = ProviderStatus.Pending;
            }
            else
            {
                userToRegister.Role = "User";
                userToRegister.ProviderStatus = null;

            }
            userToRegister.OtpExpiresAt = otpExpiresAt;
            userToRegister.OtpCode = otp;

            await _unitOfWork.GetRepository<User>().InsertAsync(userToRegister);
            await _unitOfWork.CommitAsync();

            var replacements = new Dictionary<string, string>
            {
                { "UserName", userToRegister.FullName },
                { "OtpCode", otp }
            };
            await _mailSenderService.SendEmailWithTemplateAsync(
            userToRegister.Email,
            "Xác thực tài khoản SportGo",
            "OtpVerification",
            replacements
            );
        }

        public async Task RequestForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.Email == dto.Email);
            if (user == null)
            {
                throw new ArgumentException("Email không tồn tại trong hệ thống.");
            }
            if (user != null && user.IsEmailVerified)
            {
                var otp = new Random().Next(100000, 999999).ToString();
                user.OtpCode = otp;
                user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
                _unitOfWork.GetRepository<User>().UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                var replacements = new Dictionary<string, string>
                {
                { "UserName", user.FullName },
                { "OtpCode", otp }
                };
                await _mailSenderService.SendEmailWithTemplateAsync(
                    user.Email,
                    "Yêu cầu Đặt lại mật khẩu SportGo",
                    "ForgotPasswordOtp", // Tên template mới
                    replacements
                );
            }


        }

        public async Task<bool> ResendVerificationEmailAsync(string email)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.Email == email);
            if (user == null)
            {
                throw new ArgumentException("Email không tồn tại trong hệ thống.");
            }
            if (user.IsEmailVerified)
            {
                throw new ArgumentException("Email đã được xác thực.");
            }
            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5);
            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            var replacements = new Dictionary<string, string>
            {
                { "UserName", user.FullName },
                { "OtpCode", otp }
            };
            await _mailSenderService.SendEmailWithTemplateAsync(
                user.Email,
                "Xác thực lại tài khoản SportGo",
                "OtpVerification",
                replacements
            );
            return await Task.FromResult(true);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.Email == dto.Email);
            if (user == null)
                return false;

            if (user.OtpCode != dto.Otp || user.OtpExpiresAt < DateTime.UtcNow)
            {
                return false;
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.OtpCode = null;
            user.OtpExpiresAt = null;
            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task SubmitProviderProfileAsync(int userId, CreateProviderProfileDto profileDto)
        {
            var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.UserId == userId);
            if (user == null || user.Role != "Provider")
            {
                throw new ArgumentException("Tài khoản không hợp lệ hoặc không có quyền thực hiện hành động này.");
            }
            var existingProfile = await _unitOfWork.GetRepository<ProviderProfile>().SingleOrDefaultAsync(predicate: p => p.UserId == userId);
            if (existingProfile != null)
            {
                throw new InvalidOperationException("Bạn đã nộp hồ sơ trước đó. Vui lòng chờ quản trị viên xem xét.");
            }
            var providerProfile = _mapper.Map<ProviderProfile>(profileDto);
            providerProfile.UserId = userId;
            providerProfile.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.GetRepository<ProviderProfile>().InsertAsync(providerProfile);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> VerifyEmailAsync(VerifyEmailDto verifyDto)
        {
            var user = _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(predicate: u => u.Email == verifyDto.Email && u.OtpCode == verifyDto.Otp).Result;
            if (user == null)
            {
                throw new ArgumentException("Email hoặc mã OTP không hợp lệ.");
            }
            if (user.OtpExpiresAt < DateTime.UtcNow)
            {
                throw new ArgumentException("Mã OTP đã hết hạn.");
            }
            user.IsEmailVerified = true;
            user.OtpCode = null;
            user.OtpExpiresAt = null;
            _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.CommitAsync();
            return await Task.FromResult(true);
        }



    }
}
