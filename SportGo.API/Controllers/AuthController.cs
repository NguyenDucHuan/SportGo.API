using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportGo.Service.DTOs.UserDtos; // Giả sử UserProfileDto ở đây
using SportGo.Service.DTOs.UserDtos.Authen;
using SportGo.Service.Services.Interfaces;

namespace SportGo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] // Chỉ định rằng controller này luôn trả về JSON
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _userService.RegisterAsync(registerDto);
                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn. Vui lòng kiểm tra và xác thực." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây
                return StatusCode(500, new { error = "Đã xảy ra lỗi trong quá trình đăng ký." });
            }
        }

        [HttpPost("verify-email")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyDto)
        {
            var isSuccess = await _userService.VerifyEmailAsync(verifyDto);

            if (isSuccess)
            {
                return Ok(new { message = "Xác thực email thành công. Bây giờ bạn có thể đăng nhập." });
            }

            return BadRequest(new { error = "Mã OTP không hợp lệ hoặc đã hết hạn." });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var tokens = await _userService.LoginAsync(loginDto);
                return Ok(tokens);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var tokens = await _tokenService.RefreshTokensAsync(request.RefreshToken);
                return Ok(tokens);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Nếu token không hợp lệ
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            await _tokenService.RevokeRefreshTokenAsync(request.RefreshToken);
            return Ok(new { message = "Đăng xuất thành công." });
        }

        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetCurrentUserAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "Người dùng không được tìm thấy." });
            }

            var userDto = _mapper.Map<UserProfileDto>(user);
            return Ok(userDto);
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await _userService.RequestForgotPasswordAsync(dto);
            // Luôn trả về 200 OK để tránh dò email
            return Ok(new { message = "Nếu email của bạn tồn tại trong hệ thống, chúng tôi đã gửi một mã để đặt lại mật khẩu." });
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var isSuccess = await _userService.ResetPasswordAsync(dto);

            if (isSuccess)
            {
                return Ok(new { message = "Đặt lại mật khẩu thành công. Bây giờ bạn có thể đăng nhập với mật khẩu mới." });
            }

            return BadRequest(new { error = "Mã OTP không hợp lệ hoặc đã hết hạn." });
        }

        [HttpPost("resend-verification-email")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] string Email)
        {
            var isSuccess = await _userService.ResendVerificationEmailAsync(Email);
            if (isSuccess)
            {
                return Ok(new { message = "Mã OTP mới đã được gửi đến email của bạn." });
            }
            return BadRequest(new { error = "Không thể gửi lại mã OTP. Vui lòng kiểm tra email hoặc thử lại sau." });
        }

    }
}
