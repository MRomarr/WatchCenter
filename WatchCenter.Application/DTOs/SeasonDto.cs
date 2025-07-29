namespace WatchCenter.Application.DTOs
{
    public class SeasonDto
    {

        public string Id { get; set; }
        public int SeasonNumber { get; set; }
        public ICollection<EpisodeDto> Episodes { get; set; } = new List<EpisodeDto>();

    }
}