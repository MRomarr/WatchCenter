namespace WatchCenter.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesAsync()
        {
            var result = await _movieService.GetAllAsync();
            
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles =RoleHelper.Admin)]
        public async Task<IActionResult> AddMovie([FromForm] AddMovieDto dto)
        {
            var result = await _movieService.AddMovieAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
