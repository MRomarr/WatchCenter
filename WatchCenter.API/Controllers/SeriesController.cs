
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

        //get seasons epsodes
        //add season
        //app edpsoide

        
    }
}
