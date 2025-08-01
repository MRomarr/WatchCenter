namespace WatchCenter.Application.DTOs
{
    public class SeriesDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public int Type { get; set; }
        public ICollection<SeasonDto> Seasons { get; set; } = new List<SeasonDto>();
    }
}