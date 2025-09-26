using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportGo.Service.DTOs.UserDtos.Authen;
using SportGo.Repository.Enum;
using SportGo.Service.Services.Interfaces;

namespace SportGo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProviderController : ControllerBase
    {

        private readonly IUserService _userService;
        public ProviderController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("submit-profile")]
        [Authorize(Roles = nameof(RoleEnum.Provider))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitProfile([FromBody] CreateProviderProfileDto profileDto)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { error = "Token không hợp lệ hoặc không chứa UserId." });
            }
            try
            {
                await _userService.SubmitProviderProfileAsync(userId, profileDto);
                return Ok(new { message = "Hồ sơ của bạn đã được gửi thành công. Vui lòng chờ quản trị viên xem xét." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Đã xảy ra lỗi không mong muốn khi gửi hồ sơ." });
            }
        }


    }
}
