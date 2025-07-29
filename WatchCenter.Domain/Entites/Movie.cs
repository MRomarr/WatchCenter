namespace WatchCenter.Domain.Entites
{
    public class Movie : BaseEntity
    {
        public string MovieUrl { get; set; }

        public string ContentId { get; set; }
        public Content Content { get; set; }
    }
}