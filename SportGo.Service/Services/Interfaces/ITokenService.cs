using SportGo.Repository.Entities;
using SportGo.Service.DTOs.UserDtos.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Service.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateTokensAsync(User user, string deviceId, string deviceName);
        Task<TokenResponseDto> RefreshTokensAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
