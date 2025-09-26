using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportGo.Repository.Paginate;
using SportGo.Service.DTOs.UserDtos.AdminDtos;
using SportGo.Repository.Enum;
using SportGo.Service.Services.Interfaces;

namespace SportGo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(RoleEnum.Admin))]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("providers/{userId}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveProvider(int userId)
        {
            try
            {
                await _adminService.ApproveProviderAsync(userId);
                return Ok(new { message = "Hồ sơ đã được phê duyệt thành công và email thông báo đã được gửi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("providers/{userId}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectProvider(int userId, [FromBody] RejectProfileDto rejectDto)
        {
            try
            {
                await _adminService.RejectProviderAsync(userId, rejectDto);
                return Ok(new { message = "Hồ sơ đã được từ chối thành công và email thông báo đã được gửi." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("providers")]
        [ProducesResponseType(typeof(IPaginate<ProviderPendingDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProviders([FromQuery] ProviderFilterDto filter)
        {
            var pagedProviders = await _adminService.GetFilteredProvidersAsync(filter);

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(new
            {
                pagedProviders.Total,
                pagedProviders.Size,
                pagedProviders.Page,
                pagedProviders.TotalPages
            }));

            return Ok(pagedProviders.Items);
        }

        [HttpGet("providers/{userId}")]
        [ProducesResponseType(typeof(ProviderProfileDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProviderDetail(int userId)
        {
            try
            {
                var profile = await _adminService.GetProviderProfileDetailAsync(userId);
                return Ok(profile);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
