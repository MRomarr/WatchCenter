namespace WatchCenter.Domain.Entites
{
    public class Season : BaseEntity
    {
        public int SeasonNumber { get; set; }
        public string SeriesId { get; set; }
        public Series Series { get; set; }
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();

    }
}