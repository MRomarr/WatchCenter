
namespace WatchCenter.Application.Interface.services
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterDto dto);
        Task<Result<AuthDto>> LoginAsync(LoginDto dto);
        Task<Result<AuthDto>> ConfirmEmailAsync(string userId, string token);
        Task<Result<AuthDto>> RefreshTokenAsync(string token);
        Task<Result> LogoutAsync(string token);

    }
}
