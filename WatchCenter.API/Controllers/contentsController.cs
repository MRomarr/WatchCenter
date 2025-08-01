namespace WatchCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class contentsController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;
        private readonly UserManager<ApplicationUser> _userManager;
        public contentsController(IContentService contentService, UserManager<ApplicationUser> userManager, IMovieService movieService, ISeriesService seriesService)
        {
            _contentService = contentService;
            _userManager = userManager;
            _movieService = movieService;
            _seriesService = seriesService;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id) 
        {
            var result = await _contentService.GetByIdAsync(id);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            if (result.Data.Type == 0)
            {
                //get movie dto
                var content = await _movieService.GetByIdAsync(result.Data.Id);

                if (!result.Succeeded)
                    return BadRequest(result.Message);

                return Ok(content.Data);

            }
            else if (result.Data.Type == 1)
            {
                var content = await _seriesService.GetByIdAsync(result.Data.Id);

                if (!result.Succeeded)
                    return BadRequest(result.Message);

                return Ok(content.Data);


            }
            return BadRequest("Not Found");
        } 




        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string query, [FromQuery] int Page, [FromQuery] int PageSize)
        {
            var result = await _contentService.SearchAsync(query,Page,PageSize);
            
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }




        [HttpPost]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> CreateAsync([FromForm] CreateContentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("User not found.");

            var result = await _contentService.CreateAsync(user.Id,dto);
            
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }




        [HttpPut]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateContentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _contentService.UpdateAsync(dto);

            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }




        [HttpDelete]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _contentService.DeleteAsync(id);
            if(!result.Succeeded)
                return BadRequest(result.Message);
            return Ok(result.Message);

        }
    }
}
