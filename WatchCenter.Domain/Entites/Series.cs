namespace WatchCenter.Domain.Entites
{
    public class Series : BaseEntity
    {
        public string ContentId { get; set; }
        public Content Content { get; set; }

        public ICollection<Season> Seasons { get; set; } = new List<Season>();
    }
}