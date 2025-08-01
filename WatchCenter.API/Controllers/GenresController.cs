namespace WatchCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _genreService.GetAllGenresAsync();

            return Ok(result.Data);
        }




        [HttpPost]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody] string GenreName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _genreService.CreateGenreAsync(GenreName);
            if (!result.Succeeded)
                return BadRequest(new Result<GenreDto>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            return Ok(result.Data);
        }




        [HttpPut]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGenreDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _genreService.UpdateGenreAsync(dto);
            if (!result.Succeeded)
                return BadRequest(new Result<GenreDto>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            return Ok(result.Data);
        }




        [HttpDelete("{id}")]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Genre ID cannot be null or empty.");

            var result = await _genreService.DeleteGenreAsync(id);
            if (!result.Succeeded)
                return BadRequest(new Result<GenreDto>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });

            return Ok(result.Message);
        }




        [HttpGet("{id}/contents")]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> GetContentsByGenreIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Genre ID cannot be null or empty.");

            var result = await _genreService.GetContentsByGenreIdAsync(id);
            if (!result.Succeeded)
                return BadRequest(new Result<ICollection<ContentDto>>()
                {
                    Message = result.Message,
                    Errors = result.Errors
                });
            return Ok(result.Data);

        }
    }
}
