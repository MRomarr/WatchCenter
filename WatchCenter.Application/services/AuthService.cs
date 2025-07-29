

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WatchCenter.Application.services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITokenProvider _tokenProvider;

        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailService, ITokenProvider tokenProvider)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenProvider = tokenProvider;
        }
        public async Task<Result> RegisterAsync(RegisterDto dto)
        {
            // Validate the DTO
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return Result.Failure("Registration failed", "Email already exists.");
            }
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
            {
                return Result.Failure("Registration failed", "Username already exists.");
            }

            // Create the user
            var user = new ApplicationUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result.Failure("Registration failed", errors);
            }

            //add user to default role
            if (!await _userManager.IsInRoleAsync(user, RoleHelper.User))
            {
                await _userManager.AddToRoleAsync(user, RoleHelper.User);
            }

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Create confirmation link
            var confirmationLink =
                $"https://yourapp.com/confirm-email?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}";

            // Send confirmation email
            var emailContent = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
            var emailResult = await _emailService.SendEmailAsync(dto.Email, "Confirm your email", emailContent);
            if (!emailResult)
            {
                return Result.Failure("Registration failed", "Failed to send confirmation email.");
            }

            // Return success result
            return Result.Success("User registered successfully Go Check Your Email For Verfiy");
        }
        public async Task<Result<AuthDto>> LoginAsync(LoginDto dto)
        {
            // Validate the DTO
            var user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            if (user is null)
                user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);
            if (user is null)
                return Result<AuthDto>.Failure("Login failed", "Invalid username or password.");

            // Check user Password
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result)
                return Result<AuthDto>.Failure("Login failed", "Invalid username or password.");

            // Check if email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create confirmation link
                var confirmationLink =
                    $"https://yourapp.com/confirm-email?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}";

                // Send confirmation email
                var emailContent = $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>";
                var emailResult = await _emailService.SendEmailAsync(user.Email, "Confirm your email", emailContent);
                if (!emailResult)
                {
                    return Result<AuthDto>.Failure("Registration failed", "Failed to send confirmation email.");
                }

                // Return failure result with email confirmation message
                return Result<AuthDto>.Failure("Login failed", "Email not confirmed. Please check your email for confirmation link.");
            }

            // Generate JWT access token
            var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
            if (string.IsNullOrEmpty(accessToken.ToString()))
            {
                return Result<AuthDto>.Failure("Login failed", "Failed to generate token.");
            }

            // Generate refresh token
            var refreshToken = await _tokenProvider.GenerateRefreshTokenAsync(user); 
            if (string.IsNullOrEmpty(refreshToken.ToString()))
            {
                return Result<AuthDto>.Failure("Login failed", "Failed to generate refresh token.");
            }

            // Return success result with token
            return Result<AuthDto>.Success(new AuthDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken) ,
                RefreshToken = refreshToken.Token

            }, "Login successful.");


        }
        public async Task<Result<AuthDto>> ConfirmEmailAsync(string userId, string token)
        {
            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<AuthDto>.Failure("User not found.");
            
            // Confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result<AuthDto>.Failure("Email confirmation failed", errors);
            }

            // Generate JWT access token
            var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
            if (string.IsNullOrEmpty(accessToken.ToString()))
            {
                return Result<AuthDto>.Failure("Login failed", "Failed to generate token.");
            }

            // Generate refresh token
            var refreshToken = await _tokenProvider.GenerateRefreshTokenAsync(user);
            if (string.IsNullOrEmpty(refreshToken.ToString()))
            {
                return Result<AuthDto>.Failure("Login failed", "Failed to generate refresh token.");
            }

            // Return success result with token
            return Result<AuthDto>.Success(new AuthDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.Token

            }, "Login successful.");

        }
        public async Task<Result<AuthDto>> RefreshTokenAsync(string token)
        {
            
            // Get the user from the token
            var user =
                await _userManager.Users.SingleOrDefaultAsync(t=>t.refreshTokens.Any(r=>r.Token==token));
            if (user is null)
                return Result<AuthDto>.Failure("Invalid token.", "User not found for the provided token.");

            // Find the refresh token for the user
            var refreshToken = user.refreshTokens.SingleOrDefault(r => r.Token == token);
            if (refreshToken is null)
                return Result<AuthDto>.Failure("Invalid token.", "Refresh token not found for the user.");

            // Check if the refresh token is expired
            if (!refreshToken.IsActive)
                return Result<AuthDto>.Failure("Invalid token.", "Refresh token has expired.");
            
            // revoke token to create new one
            refreshToken.RevokedOn = DateTime.UtcNow.AddMinutes(-1);
            
            // Generate new JWT access token
            var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user);
            if (string.IsNullOrEmpty(accessToken.ToString()))
                return Result<AuthDto>.Failure("Failed to generate new access token.");
            
            // Generate new refresh token
            var NewRefreshToken = await _tokenProvider.GenerateRefreshTokenAsync(user);
            if (string.IsNullOrEmpty(NewRefreshToken.ToString()))
                return Result<AuthDto>.Failure("Failed to generate new refresh token.");

            // Return success result with new tokens
            return Result<AuthDto>.Success(new AuthDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = NewRefreshToken.Token
            }, "Tokens refreshed successfully.");
        }
        public async Task<Result> LogoutAsync(string token)
        {
            // Get the user from the token
            var user =
                await _userManager.Users.SingleOrDefaultAsync(t => t.refreshTokens.Any(r => r.Token == token));
            if (user is null)
                return Result.Failure("Invalid token.", "User not found for the provided token.");

            // Find the refresh token for the user
            var refreshToken = user.refreshTokens.SingleOrDefault(r => r.Token == token);
            if (refreshToken is null)
                return Result.Failure("Invalid token.", "Refresh token not found for the user.");

            // Revoke the refresh token
            refreshToken.RevokedOn = DateTime.UtcNow.AddMinutes(-1);

            // Save changes to the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result.Failure("Logout failed", errors);
            }

            // Return success result
            return Result.Success("User logged out successfully.");
        }
    }
}
