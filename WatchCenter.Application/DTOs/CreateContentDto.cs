
namespace WatchCenter.Application.DTOs
{
    public class CreateContentDto
    {
        [Required]
        public string GenreId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        [Range(0, 1)]
        public int Type { get; set; }
    }
}
