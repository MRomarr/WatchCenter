namespace WatchCenter.Domain.Entites
{
    public class Episode : BaseEntity
    {
        public int EpisodeNumber { get; set; }
        public string EpisodeUrl { get; set; }
      
        public string SeasonId { get; set; }
        public Season Season { get; set; }
    }
}