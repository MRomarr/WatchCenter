
namespace WatchCenter.Application.DTOs
{
    public class CreateContentDto
    {
        
        public string GenreId { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public IFormFile Poster { get; set; }
        public IFormFile Trailer { get; set; }
        [Range(0, 1)]
        public int Type { get; set; }
    }
}
