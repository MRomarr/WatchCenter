namespace WatchCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Validate the DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the AuthService to register the user
            var result = await _authService.RegisterAsync(dto);

            // Check if the registration was successful
            if (!result.Succeeded)
                return BadRequest(new Result
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            // Return success response
            return Ok(result.Message);

        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Validate the DTO
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the AuthService to login the user
            var result = await _authService.LoginAsync(dto);

            // Check if the login was successful
            if (!result.Succeeded)
                return BadRequest(new Result<AuthDto>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            // Return success response with token
            return Ok(new Result<AuthDto>()
            {
                Message = result.Message,
                Data= result.Data
            });
        }




        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            // Validate the input
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("Invalid user ID or token.");

            // Call the AuthService to confirm the email
            var result = await _authService.ConfirmEmailAsync(userId, token);

            // Check if the confirmation was successful
            if (!result.Succeeded)
                if (!result.Succeeded)
                    return BadRequest(new Result<AuthDto>()
                    {
                        Message = result.Message,
                        Errors = result.Errors
                    });


            // Return success response
            return Ok(new Result<AuthDto>()
            {
                Message = result.Message,
                Data = result.Data
            });
        }




        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            // Validate the input
            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid token.");

            // Call the AuthService to refresh the token
            var result = await _authService.RefreshTokenAsync(token);

            // Check if the refresh was successful
            if (!result.Succeeded)
                return BadRequest(new Result<AuthDto>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            // Return success response with new token
            return Ok(new Result<AuthDto>()
            {
                Message = result.Message,
                Data = result.Data
            });
        }




        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] string token)
        {
            // Validate the input
            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid token.");

            // Call the AuthService to logout the user
            var result =await _authService.LogoutAsync(token);

            // Return success response
            return Ok(new Result()
            {
                Message = "User logged out successfully."
            });
        }
    }
}
