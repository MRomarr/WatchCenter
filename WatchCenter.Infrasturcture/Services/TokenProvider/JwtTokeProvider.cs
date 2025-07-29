using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WatchCenter.Infrasturcture.Services.TokenProvider
{
    public class JwtTokeProvider : ITokenProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokeProvider(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }
        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var userClimas = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClimas = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var climas = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
            };
            climas.AddRange(userClimas);
            climas.AddRange(rolesClimas);

            var issuer = _jwtSettings.Issuer ?? throw new InvalidOperationException("JWT issuer is missing.");
            var audience = _jwtSettings.Audience ?? throw new InvalidOperationException("JWT audience is missing.");
            var key = _jwtSettings.Secret ?? throw new InvalidOperationException("JWT key is missing.");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: climas,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: signingCredentials
            );
            return jwtToken;
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
            };
            user.refreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return refreshToken;
        }
    }
}
