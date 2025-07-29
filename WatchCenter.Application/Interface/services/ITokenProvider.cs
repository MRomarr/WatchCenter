using System.IdentityModel.Tokens.Jwt;

namespace WatchCenter.Application.Interface.services
{
    public interface ITokenProvider
    {
        Task<JwtSecurityToken> GenerateAccessTokenAsync(ApplicationUser user);
        Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user);
    }
}
