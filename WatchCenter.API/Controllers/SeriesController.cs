namespace WatchCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : Controller
    {
        private readonly ISeriesService _SeriesService;

        public SeriesController(ISeriesService seriesService)
        {
            _SeriesService = seriesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsyncAsync()
        {
            var result = await _SeriesService.GetAllAsync();

            return Ok(result.Data);
        }




        [HttpGet("{seriesId}/Season{SeasonId}/epsodes")]
        public async Task<IActionResult> GetSeasonEpsodes(string seriesId, string SeasonId)
        {
            var result = await _SeriesService.GetEpsodesAsync(seriesId,SeasonId);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }




        [HttpPost("{seriesId}/Season")]
        [Authorize(Roles =RoleHelper.Admin)]
        public async Task<IActionResult> CreateSeasonAsync(string seriesId)
        {
            var result = await _SeriesService.CreateSeasonAsync(seriesId);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
