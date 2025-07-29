namespace WatchCenter.Application.DTOs
{
    public class SeriesDto
    {
        public string Id { get; set; } 
        public ICollection<SeasonDto> Seasons { get; set; } = new List<SeasonDto>();
    }
}