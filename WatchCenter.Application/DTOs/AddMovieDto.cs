
namespace WatchCenter.Application.DTOs
{
    public class AddMovieDto
    {
        public string ContentId { get; set; }
        public IFormFile Movie { get; set; }
    }
}
